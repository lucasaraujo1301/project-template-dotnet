using System.Diagnostics;
using System.Text;
using System.Text.Json.Nodes;

namespace ProjectTemplate.Middlewares;

public sealed class RequestResponseLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestResponseLoggingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger = logger;

    private const int MaxBodyLength = 2000;

    private static readonly HashSet<string> _sensitiveFields = new(StringComparer.OrdinalIgnoreCase)
    {
        "password",
        "cpf",
        "token",
        "access_token",
        "refresh_token"
    };

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        // Correlation ID
        var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();

        context.Response.Headers["X-Correlation-ID"] = correlationId;
        context.TraceIdentifier = correlationId;

        var user = context.User?.Identity?.IsAuthenticated == true ? context.User.Identity!.Name : "Anonymous";

        // var ip = context.Connection.RemoteIpAddress?.ToString();

        context.Request.EnableBuffering();

        var requestBody = await ReadBodyAsync(context.Request);
        var maskedRequest = Truncate(MaskJson(requestBody));

        var headers = MaskHeaders(context.Request.Headers);

        var originalBody = context.Response.Body;
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        stopwatch.Stop();

        responseBody.Position = 0;
        var responseText = await new StreamReader(responseBody).ReadToEndAsync();
        var maskedResponse = Truncate(MaskJson(responseText));

        responseBody.Position = 0;
        await responseBody.CopyToAsync(originalBody);
        context.Response.Body = originalBody;

        var statusCode = context.Response.StatusCode;

        var logLevel = statusCode switch
        {
            >= 500 => LogLevel.Error,
            >= 400 => LogLevel.Warning,
            _ => LogLevel.Information
        };

        _logger.Log(logLevel, """
        HTTP {Method} {Path}
        CorrelationId: {CorrelationId}
        User: {User}
        Status: {Status}
        DurationMs: {Duration}
        Headers: {@Headers}
        Request: {Request}
        Response: {Response}
        """,
        context.Request.Method,
        context.Request.Path,
        correlationId,
        user,
        statusCode,
        stopwatch.ElapsedMilliseconds,
        headers,
        maskedRequest,
        maskedResponse);
    }

    private static async Task<string> ReadBodyAsync(HttpRequest request)
    {
        if (request.ContentType?.Contains("application/json") != true)
        {
            return string.Empty;
        }

        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;
        return body;
    }

    private static Dictionary<string, string> MaskHeaders(IHeaderDictionary headers)
    {
        return headers.ToDictionary(
            h => h.Key,
            h => h.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase) ? "***MASKED***" : h.Value.ToString()
        );
    }

    private static string Truncate(string value)
    {
        if (value.Length <= MaxBodyLength)
        {
            return value;
        }

        return string.Create(MaxBodyLength + 3, value, (dest, src) =>
        {
            src.AsSpan(0, MaxBodyLength).CopyTo(dest);
            ".".AsSpan().CopyTo(dest[MaxBodyLength..]);
            ".".AsSpan().CopyTo(dest[(MaxBodyLength + 1)..]);
            ".".AsSpan().CopyTo(dest[(MaxBodyLength + 2)..]);
        });
    }

    private static string MaskJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return json;
        }

        try
        {
            var node = JsonNode.Parse(json);
            if (node is null)
            {
                return json;
            }

            Mask(node);
            return node.ToJsonString();
        }
        catch
        {
            return "[Invalid JSON]";
        }
    }

    private static void Mask(JsonNode? node)
    {
        if (node is null)
        {
            return;
        }

        switch (node)
        {
            case JsonObject obj:
                foreach (var prop in obj.ToList())
                {
                    if (_sensitiveFields.Contains(prop.Key))
                    {
                        obj[prop.Key] = "*****";
                    }
                    else
                    {
                        Mask(prop.Value);
                    }
                }
                break;

            case JsonArray array:
                foreach (var item in array)
                {
                    Mask(item);
                }

                break;
        }
    }
}
