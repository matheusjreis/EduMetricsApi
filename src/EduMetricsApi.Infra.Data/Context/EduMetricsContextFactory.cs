using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EduMetricsApi.Infra.Data.Context;

public class EduMetricsContextFactory : IDesignTimeDbContextFactory<EduMetricsContext>
{
    public EduMetricsContext CreateDbContext()
    {
        string[] args = { "" };
        return CreateDbContext(args);
    }

    

    public EduMetricsContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EduMetricsContext>();

        //optionsBuilder.UseNpgsql(connection);
                      //.EnableSensitiveDataLogging(false)
                      //.EnableDetailedErrors(false);

        return new EduMetricsContext(optionsBuilder.Options, null);
    }
}
