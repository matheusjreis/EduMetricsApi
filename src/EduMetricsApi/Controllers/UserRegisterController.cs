using EduMetricsApi.Application.DTO;
using EduMetricsApi.Application.Interfaces;
using EduMetricsApi.Domain.Entities;
using EduMetricsApi.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduMetricsApi.Controllers;

[ApiController]
[Route("user-register")]
[Authorize]
public class UserRegisterController : ControllerBase
{
    private readonly IApplicationServiceBase<UserRegister, UserRegisterDto> _applicationService;
    private readonly IApplicationServiceUser _applicationServiceUser;

    public UserRegisterController(IApplicationServiceBase<UserRegister, UserRegisterDto> applicationService, IApplicationServiceUser applicationServiceUser)
    {
        _applicationService = applicationService;
        _applicationServiceUser = applicationServiceUser;
    }

    [HttpGet]
    public IActionResult Get(int id) => new EduMetricsApiResult(_applicationService.Get(id));
    
    [HttpGet("/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email) => new EduMetricsApiResult(await _applicationServiceUser.GetUserByEmail(email));

    [HttpPost]
    [AllowAnonymous]
    public IActionResult Post([FromBody] UserRegisterDto model) => new EduMetricsApiResult(_applicationServiceUser.RegisterNewUser(model));
}