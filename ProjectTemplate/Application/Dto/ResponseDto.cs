namespace ProjectTemplate.Application.Dto;


public class ResponseDto<T, K>
{
    public bool IsSuccess { get; set; } = true;
    public Dictionary<string, K>? Errors { get; set; }
    public int StatusCode { get; set; } = StatusCodes.Status200OK;
    public T? Data { get; set; }

    public static ResponseDto<T, K> Success(T data, int statusCode = StatusCodes.Status200OK) => new() { Data = data, StatusCode = statusCode };
    public static ResponseDto<T, K> Failure(Dictionary<string, K> errors, int statusCode) => new()
    {
        IsSuccess = false,
        Errors = errors,
        StatusCode = statusCode
    };
}

public class ModelPaginatedDto<TModel>
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<TModel> Items { get; set; } = [];
}
