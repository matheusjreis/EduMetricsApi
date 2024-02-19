namespace EduMetricsApi.Domain.Entities;

public class UserRegister
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string RegistrationCollegeCode { get; set; }
}