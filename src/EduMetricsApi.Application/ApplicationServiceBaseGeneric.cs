using AutoMapper;
using EduMetricsApi.Application.Interfaces;
using EduMetricsApi.Domain.Core.Services.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static EduMetricsApi.Application.Exceptions.EduMetricsApiException;

namespace EduMetricsApi.Application;

public class ApplicationServiceBaseGeneric<T, Z> : IApplicationServiceBaseGeneric<T, Z> where T : class where Z : class
{
    private readonly IServiceBaseGeneric<T> _service;
    private readonly IMapper _mapper;
    public ApplicationServiceBaseGeneric(IServiceBaseGeneric<T> service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public virtual bool Add(Z dTO)
    {
        var entity = _mapper.Map<Z, T>(dTO);
        return _service.Add(entity);
    }

    public virtual bool Add(ICollection<Z> dTO)
    {
        var entity = _mapper.Map<ICollection<Z>, ICollection<T>>(dTO);
        return _service.Add(entity);
    }

    public virtual ICollection<Z>? Get()
    {
        var entity = _service.Get();
        if (entity is null || !entity.Any())
        {
            throw new EduMetricsApiNoContentException();
        }

        return _mapper.Map<ICollection<T>, ICollection<Z>>(entity);
    }

    public virtual Z? Get(int id)
    {
        var entity = _service.Get(id);
        if (entity is null)
        {
            throw new EduMetricsApiNoContentException();
        }

        return _mapper.Map<T, Z>(entity);
    }

    public virtual ICollection<Z>? Get(Expression<Func<T, bool>> lambda)
    {
        var entity = _service.Get(lambda);
        if (entity is null || !entity.Any())
        {
            throw new EduMetricsApiNoContentException();
        }

        return _mapper.Map<ICollection<T>, ICollection<Z>>(entity);
    }

    public ICollection<Z>? GetEverything(Expression<Func<T, bool>>? lambda = null, ICollection<string>? exclude = null)
    {
        var entity = _service.Get(lambda, exclude);
        if (entity is null || !entity.Any())
        {
            throw new EduMetricsApiNoContentException();
        }

        return _mapper.Map<ICollection<T>, ICollection<Z>>(entity);
    }

    public Z? GetEverything(int id, ICollection<string>? exclude = null)
    {
        var entity = _service.Get(id, exclude);
        if (entity is null)
        {
            throw new EduMetricsApiNoContentException();
        }   

        return _mapper.Map<T, Z>(entity);
    }

    public virtual bool Update(Z dTO)
    {
        var entity = _mapper.Map<Z, T>(dTO);
        return _service.Update(entity);
    }

    public virtual bool Remove(int id)
    {
        return _service.Remove(id);
    }
}