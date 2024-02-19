using System.Linq.Expressions;

namespace EduMetricsApi.Application.Interfaces;

public interface IApplicationServiceBaseGeneric<T, Z> where T : class where Z : class
{
    public ICollection<Z>? Get();
    public Z? Get(int id);
    public ICollection<Z>? Get(Expression<Func<T, bool>> lambda);

    public ICollection<Z>? GetEverything(Expression<Func<T, bool>>? lambda = null, ICollection<string>? exclude = null);
    public Z? GetEverything(int id, ICollection<string>? exclude = null);

    public bool Add(Z dTO);
    public bool Add(ICollection<Z> dTO);
    public bool Update(Z dTO);
    public bool Remove(int id);
}