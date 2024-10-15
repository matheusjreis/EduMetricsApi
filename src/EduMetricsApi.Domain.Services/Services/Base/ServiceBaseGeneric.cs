using EduMetricsApi.Domain.Core.Context;
using EduMetricsApi.Domain.Core.Repositories.Base;
using EduMetricsApi.Domain.Core.Services.Base;
using System.Linq.Expressions;
using static EduMetricsApi.Application.Exceptions.EduMetricsApiException;

namespace EduMetricsApi.Domain.Services.Services.Base;

// TODO: MAKE ALL THOSE METHODS INTO ASYNC ONES.
public class ServiceBaseGeneric<T> : IServiceBaseGeneric<T> where T : class
{
    private readonly IRepositoryBaseGeneric<T> _repository;
    private readonly IUserContext _userContext;

    public ServiceBaseGeneric(IRepositoryBaseGeneric<T> repository, IUserContext userContext)
    {
        _repository = repository;
        _userContext = userContext;
    }

    public T GetByAlternateId(int id)
    {
        var entity = _repository.GetByAlternateId(id);

        if (entity is null)
        {
            throw new EduMetricsApiNoContentException();
        }

        return entity;
    }

    public int GetNextId(Expression<Func<T, int>> keyExpression)
    {
        return _repository.GetNextId(keyExpression);
    }

    public T Get(int id, ICollection<string>? exclude = null)
    {
        var entity = _repository.Get(id, exclude);
        if (entity is null)
        {
            throw new EduMetricsApiNoContentException();
        }

        return entity;
    }

    public T GetById(int id)
    {
        var entity = _repository.GetById(id);
        if (entity is null)
        {
            throw new EduMetricsApiNoContentException();
        }

        return entity;
    }

    public async Task<T> GetAsync(int id)
    {
        var entity = await _repository.GetAsync(id);
        if (entity is null)
        {
            throw new EduMetricsApiNoContentException();
        }

        return entity;
    }

    public virtual ICollection<T> Get(Expression<Func<T, bool>>? lambda)
    {
        var entity = _repository.Get(lambda);

        if (entity is null)
        {
            throw new EduMetricsApiNoContentException();
        }

        return entity;
    }

    public virtual ICollection<T> Get(Expression<Func<T, bool>>? lambda, ICollection<string>? exclude)
    {
        var entity = _repository.Get(lambda, exclude);

        if (entity is null)
        {
            throw new EduMetricsApiNoContentException();
        }

        return entity;
    }

    public virtual ICollection<T> Get()
    {
        var entity = _repository.Get();
        if (entity is null)
        {
            throw new EduMetricsApiNoContentException();
        }

        return entity;
    }

    public virtual bool Add(T entity) => _repository.Add(entity);
    public virtual bool Add(ICollection<T> entities) => _repository.Add(entities);
    public virtual Task<bool> AddAsync(T entity) => _repository.AddAsync(entity);

    public virtual bool Update(T entity) => _repository.Update(entity);
    public virtual Task<bool> UpdateAsync(T entity) => _repository.UpdateAsync(entity);

    public virtual bool Remove(int id)
    {
        var entity = GetById(id);
        if (entity is null)
        {
            throw new EduMetricsApiNoContentException();
        }
        return _repository.Remove(entity);
    }

    public void BeginTransaction() => _repository.BeginTransaction();
    public void CommitTransaction() => _repository.CommitTransaction();
    public void RollbackTransaction() => _repository.RollbackTransaction();

    public bool Update(ICollection<T> entity)
    {
        return _repository.Update(entity);
    }
}