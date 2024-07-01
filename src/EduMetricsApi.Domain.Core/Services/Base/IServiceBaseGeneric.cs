using System.Linq.Expressions;

namespace EduMetricsApi.Domain.Core.Services.Base;

public interface IServiceBaseGeneric<T> where T : class
{
    public T Get(int id, ICollection<string>? exclude = null);
    public T GetById(int id);
    public Task<T> GetAsync(int id);

    public ICollection<T> Get();
    public ICollection<T> Get(Expression<Func<T, bool>>? lambda = null);
    public ICollection<T> Get(Expression<Func<T, bool>>? lambda = null, ICollection<string>? exclude = null);
    public int GetNextId(Expression<Func<T, int>> keyExpression);

    public bool Add(ICollection<T> entities);
    public bool Add(T entity);
    public Task<bool> AddAsync(T entity);

    public bool Update(T entity);
    public bool Update(ICollection<T> entity);
    public Task<bool> UpdateAsync(T entity);

    public bool Remove(int id);

    public void BeginTransaction();
    public void CommitTransaction();
    public void RollbackTransaction();
}
