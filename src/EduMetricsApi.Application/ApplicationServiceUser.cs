using EduMetricsApi.Application.DTO;
using EduMetricsApi.Application.Interfaces;
using EduMetricsApi.Domain.Core.Services.Base;
using EduMetricsApi.Domain.Entities;
using EduMetricsApi.Domain.Extensions;
using static EduMetricsApi.Application.Exceptions.EduMetricsApiException;

namespace EduMetricsApi.Application;

public class ApplicationServiceUser : IApplicationServiceUser
{
    public IServiceBaseGeneric<UserRegister> _serviceUserRegister;

    public ApplicationServiceUser(IServiceBaseGeneric<UserRegister> serviceUserRegister)
    {
        _serviceUserRegister = serviceUserRegister;
    }

    public async Task<bool> AuthenticateUser(UserCredentialsDto userCredentials)
    {
        UserRegister? accountByEmail = _serviceUserRegister.Get(x => x.Email == userCredentials.UserName).FirstOrDefault();

        if (accountByEmail is null)
            throw new EduMetricsApiForbiddenException();

        if (accountByEmail.Password == PasswordExtension.HashPassword(userCredentials.UserPassword))
            return await Task.FromResult(true);

        return await Task.FromResult(false);
    }
}