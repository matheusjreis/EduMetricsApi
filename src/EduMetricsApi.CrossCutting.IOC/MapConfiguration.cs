using AutoMapper;
using EduMetricsApi.Application.DTO;
using EduMetricsApi.Domain.Entities;
using EduMetricsApi.Domain.Extensions;

namespace EduMetricsApi.CrossCutting.IOC;

public class MapConfiguration
{
    public static void LoadAplicationMappers(IMapperConfigurationExpression config)
    {
        config.CreateMap<UserCredentialsDto, UserCredentials>()
            .ReverseMap();
        
        config.CreateMap<UserRegisterDto, UserRegister>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => PasswordExtension.HashPassword(src.Password)))
            .ReverseMap();
    }
}