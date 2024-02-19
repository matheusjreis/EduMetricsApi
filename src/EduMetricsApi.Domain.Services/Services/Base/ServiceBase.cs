using EduMetricsApi.Domain.Core.Context;
using EduMetricsApi.Domain.Core.Repositories.Base;
using EduMetricsApi.Domain.Core.Services.Base;
using EduMetricsApi.Domain.Services.Services.Base;

namespace EduMetricsApi.Domain.Services.Services;

public class ServiceBase<T> : ServiceBaseGeneric<T>, IServiceBase<T> where T : class
{
    public ServiceBase(IRepositoryBaseGeneric<T> repository, IUserContext userContext) : base(repository, userContext) { }
}
