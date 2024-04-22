using EduMetricsApi.Domain.Core.Context;
using EduMetricsApi.Domain.Core.Repositories.Base;
using EduMetricsApi.Infra.Data.Context;
using EduMetricsApi.Infra.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EduMetricsApi.Infra.Data.Repositories.Base;

public class RepositoryBaseGeneric<T> : IRepositoryBaseGeneric<T> where T : class
{
    private readonly IUserContext _userContext;
    private readonly EduMetricsContext _context;
    private readonly DbContextOptions<EduMetricsContext> _options;

    public RepositoryBaseGeneric(EduMetricsContext context,
                                 DbContextOptions<EduMetricsContext> options,
                                 IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
        _options = options;
    }

    public virtual bool Add(ICollection<T> entities)
    {
        _context.Set<T>().AddRange(entities);
        return _context.SaveChanges() > 0;
    }

    public virtual bool Add(T entity)
    {
        var entry = _context.Set<T>().Add(entity);
        var success = _context.SaveChanges() > 0;
        _context.Entry<T>(entry.Entity).State = EntityState.Detached;
        return success;
    }

    public virtual async Task<bool> AddAsync(T entity)
    {
        using (var ctx = new EduMetricsContext(_options, _userContext))
        {
            var entry = ctx.Set<T>().Add(entity);

            var success = (await ctx.SaveChangesAsync()) > 0;

            _context.Entry<T>(entry.Entity).State = EntityState.Detached;

            return success;
        }
    }

    public virtual ICollection<T>? Get()
    {
        return _context.Set<T>()
                       .AsNoTracking()
                       .ToList();
    }

    public virtual T? GetById(int id)
    {
        var entity = _context.Set<T>()
                             .AsQueryable()
                             .WhereGen("Id", id)
                             .FirstOrDefault();

        if (entity != null)
        {
            _context.Entry(entity!).State = EntityState.Detached;
        }

        return entity;
    }

    public virtual T? Get(int id, ICollection<string>? exclude = null)
    {
        var entity = _context.Set<T>()
                             .AsQueryable()
                             .WhereGen("Id", id)
                             .FirstOrDefault();

        if (entity != null)
        {
            _context.Entry(entity!).State = EntityState.Detached;
        }

        return entity;
    }

    public virtual async Task<T?> GetAsync(int id)
    {
        using (var ctx = new EduMetricsContext(_options, _userContext))
        {
            return await ctx.Set<T>()
                            .WhereGen("Id", id)
                            .FirstOrDefaultAsync();
        }
    }

    public virtual T? Get(int id)
    {
        var entity = _context.Set<T>()
                             .Find(id);

        if (entity != null)
        {
            _context.Entry(entity!).State = EntityState.Detached;
        }
        return entity;
    }

    public virtual ICollection<T>? Get(Expression<Func<T, bool>>? lambda)
    {
        return _context.Set<T>()
                       .Where(lambda ?? (w => 1 == 1))
                       .ToList();
    }

    public virtual ICollection<T>? Get(Expression<Func<T, bool>>? lambda, ICollection<string>? exclude)
    {
        return _context.Set<T>()
                       .Where(lambda ?? (w => 1 == 1))
                       .ToList();
    }

    public virtual T? GetByAlternateId(int id)
    {
        var entity = _context.Set<T>()
                             .WhereGen("AlternateId", id)
                             .FirstOrDefault();

        if (entity != null)
        {
            _context.Entry(entity!).State = EntityState.Detached;
        }

        return entity;
    }

    public int GetNextId(Expression<Func<T, int>> keyExpression)
    {
        return _context.Set<T>().Max(keyExpression) + 1;
    }

    public virtual async Task<bool> UpdateAsync(T entity)
    {
        using (var ctx = new EduMetricsContext(_options, _userContext))
        {
            ctx.Set<T>().Update(entity);
            return (await ctx.SaveChangesAsync()) > 0;
        }
    }

    public virtual bool Update(T entity)
    {
        _context.Set<T>().Update(entity);
        return _context.SaveChanges() > 0;
    }

    public virtual bool Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
        return _context.SaveChanges() > 0;
    }

    public void BeginTransaction()
    {
        if (_context.Database.CurrentTransaction == null)
        {
            _context.Database.BeginTransaction();
        }
    }

    public void CommitTransaction()
    {
        if (_context.Database.CurrentTransaction != null)
        {
            _context.Database.CommitTransaction();
        }
    }

    public void RollbackTransaction()
    {
        if (_context.Database.CurrentTransaction != null)
        {
            _context.Database.RollbackTransaction();
        }
    }
}
