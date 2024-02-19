using AutoMapper;
using EduMetricsApi.Application;
using EduMetricsApi.Application.DTO;
using EduMetricsApi.Application.Interfaces;
using EduMetricsApi.Domain.Core.Context;
using EduMetricsApi.Domain.Core.Repositories.Base;
using EduMetricsApi.Domain.Core.Services.Base;
using EduMetricsApi.Domain.Entities;
using EduMetricsApi.Domain.Services.Services;
using EduMetricsApi.Domain.Services.Services.Base;
using EduMetricsApi.Infra.Data.Context;
using EduMetricsApi.Infra.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text;

namespace EduMetricsApi.CrossCutting.IOC;

public static class ConfigurationIOC
{
    public static void LoadMapper(IServiceCollection services)
    {
        var autoMapper = new MapperConfiguration(config =>
        {
            config.CreateMap<UserCredentialsDto, UserCredentials>()
                  .ReverseMap();

        });

        IMapper mapper = autoMapper.CreateMapper();
        services.TryAddSingleton(mapper);
    }

    public static void LoadServices(IServiceCollection services, IConfigurationBuilder config)
    {
        services.TryAddSingleton(config);
        
        services.AddScoped<IUserContext, UserContext>();

        services.AddScoped(typeof(IApplicationServiceBase<,>), typeof(ApplicationServiceBase<,>));
        services.AddScoped(typeof(IApplicationServiceBaseGeneric<,>), typeof(ApplicationServiceBaseGeneric<,>));

        services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));
        services.AddScoped(typeof(IServiceBaseGeneric<>), typeof(ServiceBaseGeneric<>));

        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        services.AddScoped(typeof(IRepositoryBaseGeneric<>), typeof(RepositoryBaseGeneric<>));
    }

    public static void LoadDatabase(IServiceCollection services)
    {
        var connection = Environment.GetEnvironmentVariable("ROMAP_DATABASE");

        //services.AddHealthChecks()
        //  .AddNpgSql(
        //      connectionString: connection,
        //      name: "PostgreSQL",
        //      failureStatus: HealthStatus.Unhealthy,
        //      tags: new[] { "db", "postgresql" }
        //  );

        services.AddDbContext<EduMetricsContext>(options => options.UseNpgsql(connection));
        services.AddNpgsql<EduMetricsContext>(connectionString: connection);

        using (var context = services.BuildServiceProvider().GetRequiredService<EduMetricsContext>())
        {
            context.Database.Migrate();
        }
    }

    public static void LoadSwagger(IServiceCollection services, IConfiguration config)
    {
        services.AddSwaggerGen(c => c.LoadOpenApiOptions())
                .AddAuthentication(o => o.LoadAuthenticationOptions())
                .AddJwtBearer(o => o.LoadJwtBearerOptions(config));
    }

    private static void LoadOpenApiOptions(this SwaggerGenOptions options)
    {
        var securityScheme = new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JSON Web Token based security",
        };
        var securityReq = new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        };
        var contact = new OpenApiContact()
        {
            Name = "ASA Consultoria e Serviços de Tecnologia LTDA",
            Email = "contato@asaconsultoria.com.br",
            Url = new Uri("mailto:contato@asaconsultoria.com.br")
        };
        var license = new OpenApiLicense()
        {
            Name = "Romap Seguros License",
            Url = new Uri("https://www.asaconsultoria.com.br/")
        };
        var info = new OpenApiInfo()
        {
            Version = "v1",
            Title = "Romap Seguros",
            Description = "API designed to Romap Seguros Application",
            TermsOfService = new Uri("https://www.asaconsultoria.com.br/"),
            Contact = contact,
            License = license
        };

        options.SwaggerDoc("v1", info);
        options.AddSecurityDefinition("Bearer", securityScheme);
        options.AddSecurityRequirement(securityReq);
    }

    private static void LoadAuthenticationOptions(this AuthenticationOptions o)
    {
        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }

    private static void LoadJwtBearerOptions(this JwtBearerOptions o, IConfiguration config)
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true
        };
    }

    public static void LoadCronJobs(IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            var key = new JobKey("SendPackJob");
            options.UseMicrosoftDependencyInjectionJobFactory();
            options.AddJob<SendPack>(key);

            options.AddTrigger(opts => opts
                   .ForJob(key)
                   .WithIdentity("SendPackJob-trigger")
                   .StartNow()
                   .WithCronSchedule("59 59 23 L * ? *"));
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
}
