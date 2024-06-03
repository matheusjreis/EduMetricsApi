using System.ComponentModel.DataAnnotations.Schema;

namespace EduMetricsApi.Domain.Entities;

public class UserSession : EntityBase
{
    public DateTime LoginDate { get; set; }
    public DateTime ExpirationDate { get; set; }

    [ForeignKey("UserRegister")]
    public int UserId { get; set; }
    public UserRegister User { get; set; }

    public UserSession(int userId)
    {
        this.UserId = userId;
        this.LoginDate = DateTime.Now;
        this.ExpirationDate = DateTime.Now.AddDays(1);
    }
}