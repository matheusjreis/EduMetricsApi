using EduMetricsApi.Domain.Core.Context;
using EduMetricsApi.Domain.Core.Repositories.Base;
using EduMetricsApi.Domain.Entities;
using EduMetricsApi.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EduMetricsApi.Infra.Data.Repositories.Base;

public class RepositoryBase<T> : RepositoryBaseGeneric<T>, IRepositoryBase<T> where T : EntityBase
{
    private readonly EduMetricsContext _context;
    private readonly DbContextOptions<EduMetricsContext> _options;

    public RepositoryBase(EduMetricsContext context,
                          DbContextOptions<EduMetricsContext> options,
                          IUserContext userContext) : base(context, options, userContext)
    {
        _context = context;
        _options = options;
    }

    public new virtual bool Update(T entity)
    {
        var model = GetById(entity.Id);
        entity.SetBaseEntityValues(model!);
        _context.Set<T>().Update(entity);
        return _context.SaveChanges() > 0;
    }
}