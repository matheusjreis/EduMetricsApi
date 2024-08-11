using EduMetricsApi.Application.DTO;
using EduMetricsApi.Application.Interfaces;
using EduMetricsApi.Domain.Entities;
using EduMetricsApi.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduMetricsApi.Controllers;

[ApiController]
[Route("login")]
public class LoginController : ControllerBase
{
    private readonly IApplicationServiceBase<UserCredentials, UserCredentialsDto> _applicationService;
    private readonly IApplicationServiceUser _applicationServiceUser;

    public LoginController(IApplicationServiceBase<UserCredentials, UserCredentialsDto> applicationService, IApplicationServiceUser applicationServiceUser)
    {
        _applicationService = applicationService;
        _applicationServiceUser = applicationServiceUser;
    }


    //TODO: RECEIVE BROWSER INFOS ON HTTP HEADER
    [HttpPost]
    [Route("authenticate")]
    [AllowAnonymous]
    public async Task<IActionResult> Authenticate([FromBody] UserCredentialsDto user)
        => new EduMetricsApiResult(await _applicationServiceUser.AuthenticateUser(user));
}