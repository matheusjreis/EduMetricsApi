using EduMetricsApi.Domain.Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EduMetricsApi.Infra.Data.Context;

public class EduMetricsContext : DbContext
{
    private readonly IUserContext _userContext;
    private DbContextOptions<EduMetricsContext> Options { get; set; }
    public EduMetricsContext(DbContextOptions<EduMetricsContext> options, IUserContext userContext) : base(options)
    {
        this.Options = options;
        this._userContext = userContext;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings =>
        {
            warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored);
        });

        new EduMetricsContextFactory().CreateDbContext();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EduMetricsContext).Assembly);
    }
}