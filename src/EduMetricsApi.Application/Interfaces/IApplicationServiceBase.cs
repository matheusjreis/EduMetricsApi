using EduMetricsApi.Domain.Entities;

namespace EduMetricsApi.Application.Interfaces;

public interface IApplicationServiceBase<T, Z> : IApplicationServiceBaseGeneric<T, Z> where T : class where Z : class { }