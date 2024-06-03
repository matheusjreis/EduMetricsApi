using AutoMapper;
using EduMetricsApi.Application.DTO;
using EduMetricsApi.Application.Exceptions;
using EduMetricsApi.Application.Interfaces;
using EduMetricsApi.Domain.Core.Services;
using EduMetricsApi.Domain.Core.Services.Base;
using EduMetricsApi.Domain.Entities;
using EduMetricsApi.Domain.Extensions;
using static EduMetricsApi.Application.Exceptions.EduMetricsApiException;

namespace EduMetricsApi.Application;

public class ApplicationServiceUser : IApplicationServiceUser
{
    public IServiceBaseGeneric<UserRegister> _serviceUserRegister;
    public IServiceBaseGeneric<UserSession> _serviceUserSession;
    public IServiceAuth _serviceAuth;
    public readonly IMapper _mapper;

    public ApplicationServiceUser(IServiceBaseGeneric<UserRegister> serviceUserRegister
                                , IServiceBaseGeneric<UserSession> serviceUserSession
                                , IServiceAuth serviceAuth
                                , IMapper mapper)
    {
        _serviceUserRegister = serviceUserRegister;
        _serviceUserSession = serviceUserSession;
        _serviceAuth = serviceAuth;
        _mapper = mapper;
    }

    public async Task<string> AuthenticateUser(UserCredentialsDto userCredentials)
    {
        UserRegister? accountByEmail = _serviceUserRegister.Get(x => x.Email == userCredentials.UserName).FirstOrDefault();

        if (accountByEmail is not null && accountByEmail.Password == PasswordExtension.HashPassword(userCredentials.UserPassword))
        {
            _serviceUserSession.Add(new UserSession(accountByEmail.Id));
            var session = _serviceUserSession.Get(x => x.UserId == accountByEmail.Id).FirstOrDefault();

            return await Task.FromResult(_serviceAuth.GetToken(accountByEmail.Id, session.Id)) ;
        }
        else
        {
            throw new EduMetricsApiForbiddenException();
        }
    }

    public async Task<bool> RegisterNewUser(UserRegisterDto userRegister)
    {
        UserRegister? accountByEmail = _serviceUserRegister.Get(x => x.Email == userRegister.Email).FirstOrDefault();

        if(accountByEmail is not null)
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

        return _mapper.Map<UserRegister, UserRegisterDto>(accountByEmail);
    }
}