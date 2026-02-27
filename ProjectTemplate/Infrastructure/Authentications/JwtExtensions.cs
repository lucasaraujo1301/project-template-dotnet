using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ProjectTemplate.Infrastructure.Authentications;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        IConfigurationSection section = configuration.GetSection("Jwt");

        // ✅ Bind + Validate ON STARTUP
        services.AddOptions<JwtSettings>()
                .Bind(section)
                .ValidateDataAnnotations()
                .ValidateOnStart();

        JwtSettings jwtSettings = section.Get<JwtSettings>() ?? throw new InvalidOperationException("Jwt configuration is missing.");

        byte[] key = Encoding.UTF8.GetBytes(jwtSettings.Key);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            options.EventsType = typeof(JwtBearerEventsHandler);
        });

        return services;
    }
}
