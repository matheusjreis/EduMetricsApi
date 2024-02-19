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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
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

        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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
        var connection = Environment.GetEnvironmentVariable("EDUMETRICS_DATABASE");

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
        services.AddSwaggerGen(c => c.LoadOpenApiOptions());
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
            Name = "Matheus José dos Reis",
            Email = "matheus.reis@ufu.br",
            Url = new Uri("mailto:matheus.reis@ufu.br")
        };
        var license = new OpenApiLicense()
        {
            Name = "Reis License",
            Url = new Uri("mailto:matheus.reis@ufu.br")
        };
        var info = new OpenApiInfo()
        {
            Version = "v1",
            Title = "EduMetrics API UFU",
            Description = "API designed to EduMetrics API UFU",
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
}
