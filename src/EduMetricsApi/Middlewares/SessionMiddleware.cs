using System.IdentityModel.Tokens.Jwt;

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
                context.Session.SetString("userId", claims.FirstOrDefault(x => x.Type == "UserId")!.Value);
                context.Session.SetString("username", claims.FirstOrDefault(x => x.Type == "FullName")!.Value);
            }
        }

        await _next(context);
    }
}