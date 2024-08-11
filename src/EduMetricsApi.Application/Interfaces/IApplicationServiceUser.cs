using EduMetricsApi.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduMetricsApi.Application.Interfaces;

public interface IApplicationServiceUser
{
    public Task<string> AuthenticateUser(UserCredentialsDto userCredentials);
    public Task<bool> RegisterNewUser(UserRegisterDto userRegister);
    public Task<UserInformationsDto> Get(int userRegister);
    public Task<UserInformationsDto> GetUserByEmail(string userEmail);
    public Task<UserInformationsDto> GetLoggedUser();
}