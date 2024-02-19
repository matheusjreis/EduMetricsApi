using Microsoft.EntityFrameworkCore;

namespace EduMetricsApi.Infra.Data.Context;

public class EduMetricsContextFactory
{
    public EduMetricsContext CreateDbContext()
    {
        string[] args = { "" };
        return CreateDbContext(args);
    }

    public EduMetricsContext CreateDbContext(string[] args)
    {
        var connection = Environment.GetEnvironmentVariable("EDUMETRICS_DATABASE");

        var optionsBuilder = new DbContextOptionsBuilder<EduMetricsContext>();

        //optionsBuilder.UseSqlServer(connection)
        //              .EnableSensitiveDataLogging(false)
        //              .EnableDetailedErrors(false);

        return new EduMetricsContext(optionsBuilder.Options, null);
    }
}
