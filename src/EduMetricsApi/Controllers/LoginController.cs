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
[Authorize]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IApplicationServiceBase<UserCredentials, UserCredentialsDto> _applicationService;

    public LoginController(IApplicationServiceBase<UserCredentials, UserCredentialsDto> applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Get(int id) => new EduMetricsApiResult(_applicationService.Get(id));

    [HttpPost]
    public IActionResult Post([FromBody] UserCredentialsDto model) => new EduMetricsApiResult(_applicationService.Add(model));

}