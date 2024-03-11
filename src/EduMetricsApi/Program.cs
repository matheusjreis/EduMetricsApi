using EduMetricsApi.CrossCutting.IOC;
using EduMetricsApi.Middlewares;
using Microsoft.AspNetCore.Session;
using System;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
ConfigurationIOC.LoadServices(builder.Services, builder.Configuration);
ConfigurationIOC.LoadDatabase(builder.Services);
ConfigurationIOC.LoadMapper(builder.Services);
ConfigurationIOC.LoadSwagger(builder.Services, builder.Configuration);

builder.Services.AddControllers()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(policy => policy.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<HandlerExceptionApi>();

app.MapControllers();

app.Run();