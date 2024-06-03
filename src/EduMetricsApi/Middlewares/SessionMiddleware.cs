using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace EduMetricsApi.Middlewares;

public class SessionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SessionMiddleware> _logger;

    public SessionMiddleware(RequestDelegate next, ILogger<SessionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.GetTypedHeaders().Headers.Authorization;

        if (!string.IsNullOrEmpty(token))
        {
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token.ToString().Replace("Bearer", "").Trim());
            var claims = jwtSecurityToken.Claims.ToList();

            if (claims != null)
            {
                var teste = claims.FirstOrDefault(x => x.Type == "UserId");
                var teste2 = teste.Value;

                context.Items["UserId"] = Convert.ToInt32(claims.FirstOrDefault(x => x.Type == "UserId")!.Value);
                context.Items["SessionId"] = Convert.ToInt32(claims.FirstOrDefault(x => x.Type == "SessionId")!.Value);
            }
        }

        await _next(context);
    }
}