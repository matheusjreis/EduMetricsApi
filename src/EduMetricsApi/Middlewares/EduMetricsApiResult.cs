using EduMetricsApi.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduMetricsApi.Middlewares;

public class EduMetricsApiResult : IActionResult
{
    private readonly ResponseApi _response;

    public EduMetricsApiResult(object? data, string? message = null)
    {
        _response = new ResponseApi(true, message, data);
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var objectResult = new ObjectResult(_response)
        {
            StatusCode = context.HttpContext.Request.Method == "POST" ? (int)HttpStatusCode.Created : (int)HttpStatusCode.OK
        };

        await objectResult.ExecuteResultAsync(context);
    }
}