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
        this.ComputerBrowser = httpContextAccessor.HttpContext?.Request?.Headers["computerBrowser"];
        this.UserId = userId;
        this.LoginDate = DateTime.Now;
        this.ExpirationDate = DateTime.Now.AddHours(4);
    }

    public UserSession() { }
}