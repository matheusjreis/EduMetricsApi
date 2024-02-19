using AutoMapper;
using EduMetricsApi.Application.DTO;
using EduMetricsApi.Domain.Entities;

namespace EduMetricsApi.CrossCutting.IOC;

public class MapConfiguration
{
    public static void LoadAplicationMappers(IMapperConfigurationExpression config)
    {
        config.CreateMap<UserCredentialsDto, UserCredentials>()
            .ReverseMap();
        
        config.CreateMap<UserRegisterDto, UserRegister>()
            .ReverseMap();
    }
}
