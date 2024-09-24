using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduMetricsApi.Domain.Entities;

public class UserSession : EntityBase
{
    public DateTime LoginDate { get; set; }
    public DateTime ExpirationDate { get; set; }

    [ForeignKey("UserRegister")]
    public int UserId { get; set; }
    public string? ComputerIp { get; set; }
    public string? ComputerBrowser { get; set; }
    public UserRegister User { get; set; }

    public UserSession(int userId, IHttpContextAccessor httpContextAccessor)
    {
        this.ComputerIp = httpContextAccessor.HttpContext?.Request?.Headers["computerIp"];
        this.ComputerBrowser = GetBrowserName(httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"]!);
        this.UserId = userId;
        this.LoginDate = DateTime.Now;
        this.ExpirationDate = DateTime.Now.AddHours(4);
    }

    public string GetBrowserName(string userAgent)
    {
        string browser;

        if (userAgent.Contains("Opera") || userAgent.Contains("Opr"))
        {
            browser = "Opera";
        }
        else if (userAgent.Contains("Edg"))
        {
            browser = "Edge";
        }
        else if (userAgent.Contains("Chrome"))
        {
            browser = "Chrome";
        }
        else if (userAgent.Contains("Safari"))
        {
            browser = "Safari";
        }
        else if (userAgent.Contains("Firefox"))
        {
            browser = "Firefox";
        }
        else
        {
            browser = "unknown";
        }

        return browser;
    }

    public UserSession() { }
}