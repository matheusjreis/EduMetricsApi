using EduMetricsApi.Domain.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EduMetricsApi.Domain.Services.Services;

public class ServiceAuth : IServiceAuth
{
    private readonly IConfiguration _configuration;

    public ServiceAuth(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetToken(int userId, int sessionId)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var secret = _configuration["Jwt:Key"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes(secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                    new Claim("UserId",userId.ToString()),
                    new Claim("SessionId",sessionId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            }),
            Expires = DateTime.Now.AddHours(4),
            Audience = audience,
            Issuer = issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);

        return jwtTokenHandler.WriteToken(token);
    }
}