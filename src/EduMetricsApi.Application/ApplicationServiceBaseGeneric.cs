using AutoMapper;
using EduMetricsApi.Application.Interfaces;
using EduMetricsApi.Domain.Core.Services.Base;
using System.Linq.Expressions;

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
            throw new ApplicationNoContentException();
        }

        return _mapper.Map<ICollection<T>, ICollection<Z>>(entity);
    }

    public virtual ICollection<Z>? GetEverything()
    {
        var entity = _service.GetEverything();
        if (entity is null || !entity.Any())
        {
            throw new ApplicationNoContentException();
        }

        return _mapper.Map<ICollection<T>, ICollection<Z>>(entity);
    }

    public virtual Z? Get(int id)
    {
        var entity = _service.GetById(id);
        if (entity is null)
        {
            throw new ApplicationNoContentException();
        }

        return _mapper.Map<T, Z>(entity);
    }

    public virtual ICollection<Z>? Get(Expression<Func<T, bool>> lambda)
    {
        var entity = _service.Get(lambda);
        if (entity is null || !entity.Any())
        {
            throw new ApplicationNoContentException();
        }

        return _mapper.Map<ICollection<T>, ICollection<Z>>(entity);
    }

    public ICollection<Z>? GetEverything(Expression<Func<T, bool>>? lambda = null)
    {
        var entity = _service.Get(lambda);
        if (entity is null || !entity.Any())
        {
            throw new ApplicationNoContentException();
        }

        return _mapper.Map<ICollection<T>, ICollection<Z>>(entity);
    }

    public Z? GetEverything(int id)
    {
        var entity = _service.GetById(id);
        if (entity is null)
        {
            throw new ApplicationNoContentException();
        }

        return _mapper.Map<T, Z>(entity);
    }

    public virtual bool Update(Z dTO)
    {
        var entity = _mapper.Map<Z, T>(dTO);
        return _service.Update(entity);
    }

    public virtual bool Update(ICollection<Z> dTO)
    {
        var entity = _mapper.Map<ICollection<Z>, ICollection<T>>(dTO);
        return _service.Update(entity);
    }

    public ICollection<Z>? GetList(FilterModel filter)
    {
        var entity = _service.GetList(filter);
        if (entity is null)
        {
            throw new ApplicationNoContentException();
        }

        return _mapper.Map<ICollection<T>, ICollection<Z>>(entity);
    }

    public Z? GetBySellerId(int sellerId)
    {
        var entity = _service.GetBySellerId(sellerId);

        return _mapper.Map<T, Z>(entity!.FirstOrDefault()!);
    }

    public ICollection<Z> GetPaginated(Pagination page, object filter)
    {
        var entity = _service.GetPaginated(page, filter);

        if (entity is null)
        {
            throw new ApplicationNoContentException();
        }

        return _mapper.Map<ICollection<T>, ICollection<Z>>(entity);

    }

    public virtual bool Remove(int id)
    {
        return _service.Remove(id);
    }

    public bool RemoveBySellerId(int sellerId)
    {
        return _service.RemoveBySellerId(sellerId);
    }

    public Z? GetBySellerJwtToken()
    {
        UserContext section = _serviceUser.GetUserContextData();

        if (section.SellerId is null)
        {
            throw new ApplicationUserIsNotASellerException();
        }

        return GetBySellerId((int)section.SellerId);
    }
}