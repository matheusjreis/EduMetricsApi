namespace EduMetricsApi.Domain.Core.Context;

public interface IUserContext
{
    public string? Username { get; }
    public int? UserId { get; }
}