using AutoMapper;
using EduMetricsApi.Application.DTO;
using EduMetricsApi.Application.Exceptions;
using EduMetricsApi.Application.Interfaces;
using EduMetricsApi.Domain.Core.Services;
using EduMetricsApi.Domain.Core.Services.Base;
using EduMetricsApi.Domain.Entities;
using EduMetricsApi.Domain.Extensions;
using Microsoft.AspNetCore.Http;
using static EduMetricsApi.Application.Exceptions.EduMetricsApiException;

namespace EduMetricsApi.Application;

public class ApplicationServiceUser : IApplicationServiceUser
{
    public IServiceBaseGeneric<UserRegister> _serviceUserRegister;
    public IServiceBaseGeneric<UserSession> _serviceUserSession;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public IServiceAuth _serviceAuth;
    public readonly IMapper _mapper;

    public ApplicationServiceUser(IServiceBaseGeneric<UserRegister> serviceUserRegister
                                , IServiceBaseGeneric<UserSession> serviceUserSession
                                , IServiceAuth serviceAuth
                                , IHttpContextAccessor httpContextAccessor
                                , IMapper mapper)
    {
        _serviceUserRegister = serviceUserRegister;
        _httpContextAccessor = httpContextAccessor;
        _serviceUserSession = serviceUserSession;
        _serviceAuth = serviceAuth;
        _mapper = mapper;
    }

    public async Task<string> AuthenticateUser(UserCredentialsDto userCredentials)
    {
        UserRegister? accountByEmail = _serviceUserRegister.Get(x => x.Email == userCredentials.UserName).FirstOrDefault();

        if (accountByEmail is null || accountByEmail.Password != PasswordExtension.HashPassword(userCredentials.UserPassword))
            throw new EduMetricsApiForbiddenException();

        _serviceUserSession.Add(new UserSession(accountByEmail.Id));

        UserSession session = _serviceUserSession.Get(x => x.UserId == accountByEmail.Id).FirstOrDefault()!;

        return await Task.FromResult(_serviceAuth.GetToken(accountByEmail.Id, session.Id));
    }

    public async Task<bool> RegisterNewUser(UserRegisterDto userRegister)
    {
        UserRegister? accountByEmail = _serviceUserRegister.Get(x => x.Email == userRegister.Email).FirstOrDefault();

        if (accountByEmail is not null)
            throw new EduMetricsApiException("Usuário já possui cadastro!");

        UserRegister mappedRegister = _mapper.Map<UserRegisterDto, UserRegister>(userRegister);
        mappedRegister.Id = _serviceUserRegister.GetNextId(x => x.Id);

        return await Task.FromResult(_serviceUserRegister.Add(mappedRegister));
    }

    public async Task<UserRegisterDto> GetUserByEmail(string userEmail)
    {
        UserRegister? accountByEmail = _serviceUserRegister.Get(x => x.Email == userEmail).FirstOrDefault();

        if (accountByEmail is null)
            throw new EduMetricsApiNotFoundException();

        return await Task.FromResult(_mapper.Map<UserRegister, UserRegisterDto>(accountByEmail));
    }

    public async Task<UserRegisterDto?> GetLoggedUser()
    {
        _httpContextAccessor.HttpContext.Items.TryGetValue("UserId", out var userId);
        UserRegister user = _serviceUserRegister.GetById(Convert.ToInt32(userId));

        return await Task.FromResult(_mapper.Map<UserRegister, UserRegisterDto>(user));
    }
}