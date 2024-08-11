using System.ComponentModel.DataAnnotations.Schema;

namespace EduMetricsApi.Domain.Entities;

public class UserSession : EntityBase
{
    public DateTime LoginDate { get; set; }
    public DateTime ExpirationDate { get; set; }

    [ForeignKey("UserRegister")]
    public int UserId { get; set; }
    public string UserIp { get; set; }
    public string Browser { get; set; }
    public UserRegister User { get; set; }

    public UserSession(int userId, ComputerInformations computerInformations)
    {
        this.UserIp = computerInformations.UserIp;
        this.Browser = computerInformations.Browser;
        this.UserId = userId;
        this.LoginDate = DateTime.Now;
        this.ExpirationDate = DateTime.Now.AddHours(4);
    }

    public UserSession() { }
}