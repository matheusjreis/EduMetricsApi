using EduMetricsApi.Application.DTO;
using EduMetricsApi.Application.Interfaces;
using EduMetricsApi.Domain.Entities;
using EduMetricsApi.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduMetricsApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserRegisterController : ControllerBase
{
    private readonly IApplicationServiceBase<UserRegister, UserRegisterDto> _applicationService;

    public UserRegisterController(IApplicationServiceBase<UserRegister, UserRegisterDto> applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Get(int id) => new EduMetricsApiResult(_applicationService.Get(id));

    [HttpPost]
    public IActionResult Post([FromBody] UserRegisterDto model) => new EduMetricsApiResult(_applicationService.Add(model));
}