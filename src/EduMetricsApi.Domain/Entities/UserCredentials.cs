namespace EduMetricsApi.Domain.Entities;

public class UserCredentials : EntityBase
{
    public string UserName { get; set; }
    public string UserPassword { get; set; }
}