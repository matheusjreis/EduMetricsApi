using EduMetricsApi.Application.DTO;

namespace EduMetricsApi.Application.Interfaces;

public interface IApplicationServiceSession
{
    public Task<bool> IsSessionActivated();
}