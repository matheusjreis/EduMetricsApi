using System.Security.Claims;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using EduMetricsApi.Domain.Entities;

namespace EduMetricsApi.Domain.Core.Services;

public interface IServiceAuth
{
    public string GetToken(int userId, int sessionId, ComputerInformations computerInformations);
}