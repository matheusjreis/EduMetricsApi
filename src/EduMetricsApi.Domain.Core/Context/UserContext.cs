using Microsoft.AspNetCore.Http;
using System.Text;

namespace EduMetricsApi.Domain.Core.Context;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? Username
    {
        get
        {
            byte[]? username = null;
            _httpContextAccessor.HttpContext?.Session.TryGetValue("username", out username);
            return username is null ? null : Encoding.UTF8.GetString(username);
        }
    }

    public int? UserId
    {
        get
        {
            byte[]? userId = null;
            _httpContextAccessor.HttpContext?.Session.TryGetValue("userId", out userId);
            return userId is null ? null : int.Parse(Encoding.UTF8.GetString(userId));
        }
    }
}

