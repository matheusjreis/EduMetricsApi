using System.Linq.Expressions;

namespace EduMetricsApi.Domain.Core.Repositories.Base;

public interface IRepositoryBaseGeneric<T> where T : class
{
    public T? GetByAlternateId(int id);
    public Task<T?> GetAsync(int id);
    public T? Get(int id, ICollection<string>? exclude = null);
    public T? GetById(int id);

    public ICollection<T>? Get();
    public ICollection<T>? Get(Expression<Func<T, bool>>? lambda);
    public ICollection<T>? Get(Expression<Func<T, bool>>? lambda, ICollection<string>? exclude);

    public bool Add(ICollection<T> entities);
    public bool Add(T entity);
    public Task<bool> AddAsync(T entity);

    public bool Update(T entity);
    public Task<bool> UpdateAsync(T entity);

    public bool Remove(T entity);

    public void BeginTransaction();
    public void CommitTransaction();
    public void RollbackTransaction();
}