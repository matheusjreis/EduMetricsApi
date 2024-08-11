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
        ComputerInformations computerInformations = _mapper.Map<ComputerInformationsDto, ComputerInformations>(userCredentials.ComputerInformations);
        UserRegister? UserAccount = _serviceUserRegister.Get(x => x.Email == userCredentials.UserName).FirstOrDefault();

        if (UserAccount is not null && UserAccount.Password == PasswordExtension.HashPassword(userCredentials.UserPassword))
        {
            _serviceUserSession.Add(new UserSession(UserAccount.Id, computerInformations));
            UserSession? session = _serviceUserSession.Get(x => x.UserId == UserAccount.Id).FirstOrDefault();

            return await Task.FromResult(_serviceAuth.GetToken(UserAccount.Id, session!.Id, computerInformations));
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

    public async Task<UserInformationsDto> GetUserByEmail(string userEmail)
    {
        UserRegister? accountByEmail = _serviceUserRegister.Get(x => x.Email == userEmail).FirstOrDefault();

        if (accountByEmail is null)
            throw new EduMetricsApiNotFoundException();

        return _mapper.Map<UserRegister, UserInformationsDto>(accountByEmail);
    }

    public async Task<UserInformationsDto?> GetLoggedUser()
    {
        _httpContextAccessor.HttpContext.Items.TryGetValue("UserId", out var userId);
        UserRegister user = _serviceUserRegister.GetById(Convert.ToInt32(userId));
        return _mapper.Map<UserRegister, UserInformationsDto>(user);
    }

    public async Task<UserInformationsDto> Get(int userRegister)
    {
       var user = _serviceUserRegister.GetById(userRegister);

        return await Task.FromResult(_mapper.Map<UserRegister, UserInformationsDto>(user));
    }
}