namespace EduMetricsApi.Domain.Entities;

public class UserRegister : EntityBase
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string RegistrationCollegeCode { get; set; }
}