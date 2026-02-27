using System.ComponentModel.DataAnnotations;

namespace ProjectTemplate.Infrastructure.Authentications;

public sealed class JwtSettings
{
    [Required]
    public required string Key { get; init; }
    public string? Issuer { get; init; }
    public string? Audience { get; init; }
}
