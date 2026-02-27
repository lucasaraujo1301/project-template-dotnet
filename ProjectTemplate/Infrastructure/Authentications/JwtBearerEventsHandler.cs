using System.Security.Claims;

using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ProjectTemplate.Infrastructure.Authentications
{
    public class JwtBearerEventsHandler : JwtBearerEvents
    {
        public JwtBearerEventsHandler()
        {
            OnTokenValidated = context =>
            {
                string? userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    context.Fail("Invalid token");
                    return Task.CompletedTask; ;
                }

                // var user = repo.GetById(int.Parse(userId));

                // if (user == null || !user.IsActive)
                // {
                //     context.Fail("User no longer valid.");
                //     return Task.CompletedTask;
                // }

                return Task.CompletedTask;
            };
        }
    }
}
