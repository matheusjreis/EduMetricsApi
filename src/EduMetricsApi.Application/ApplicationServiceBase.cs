using AutoMapper;
using EduMetricsApi.Application.DTO;
using EduMetricsApi.Application.Interfaces;
using EduMetricsApi.Domain.Core.Services.Base;
using EduMetricsApi.Domain.Entities;
namespace EduMetricsApi.Application;

public class ApplicationServiceBase<T, Z> : ApplicationServiceBaseGeneric<T, Z>, IApplicationServiceBase<T, Z> where T  : class where Z : class
{
    public ApplicationServiceBase(IServiceBase<T> serviceBase, IMapper mapper) : base(serviceBase, mapper) { }
}