using EduMetricsApi.Domain.Core.Context;
using EduMetricsApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;

namespace EduMetricsApi.Infra.Data.Context;

public class EduMetricsContext : DbContext
{
    private readonly IUserContext _userContext;
    private DbContextOptions<EduMetricsContext> Options { get; set; }

    public DbSet<UserCredentials>? UserCredentials { get; set; }

    public EduMetricsContext(DbContextOptions<EduMetricsContext> options, IUserContext userContext) : base(options)
    {
        this.Options = options;
        this._userContext = userContext;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connection = Environment.GetEnvironmentVariable("EDUMETRICS_DATABASE")!;

        if (optionsBuilder.IsConfigured)
        {
            base.OnConfiguring(optionsBuilder);
        }
        else
        {
            optionsBuilder.UseNpgsql(connection, b => b.MigrationsAssembly("EduMetricsApi"));
        }

        optionsBuilder.ConfigureWarnings(warnings =>
        {
            warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored);
        });

        new EduMetricsContextFactory().CreateDbContext();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Ignore<ComputerInformations>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EduMetricsContext).Assembly);
    }
}