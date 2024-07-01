using EduMetricsApi.Application.DTO;
using EduMetricsApi.Application.Interfaces;
using EduMetricsApi.Middlewares;
using EduMetricsApi.Migrations;
using Microsoft.AspNetCore.Mvc;

namespace EduMetricsApi.Controllers;

[ApiController]
[Route("session")]
public class SessionController : ControllerBase
{
    private readonly IApplicationServiceSession _applicationServiceSession;

    public SessionController(IApplicationServiceSession applicationServiceSession)
    {
        _applicationServiceSession = applicationServiceSession;
    }

    [HttpPost]
    [Route("is-user-session-activated")]
    public async Task<IActionResult> Post() => new EduMetricsApiResult(await _applicationServiceSession.IsSessionActivated()); 
    
    [HttpPost]
    [Route("close")]
    public async Task<IActionResult> CloseSession() => new EduMetricsApiResult(await _applicationServiceSession.CloseSession());
}