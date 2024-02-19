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
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IApplicationServiceBase<UserCredentials, UserCredentialsDto> _applicationService;

    public LoginController(IApplicationServiceBase<UserCredentials, UserCredentialsDto> applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpPost]
    public IActionResult Post([FromBody] UserCredentialsDto model) => new EduMetricsApiResult(_applicationService.Add(model));

}