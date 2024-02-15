using EduMetricsApi.Application.Helpers;
using EduMetricsApi.Domain.Constants;
using EduMetricsApi.Domain.Exceptions;
using Newtonsoft.Json;
using System.Net;
using static EduMetricsApi.Domain.Exceptions.EduMetricsApiException;

namespace EduMetricsApi.Middlewares;

public class HandlerExceptionApi
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HandlerExceptionApi> _logger;

    public HandlerExceptionApi(RequestDelegate next, ILogger<HandlerExceptionApi> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var response = context.Response;

        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            var r = new ResponseApi(false, e.Message);

            response.ContentType = "application/json; charset=utf-8";
            response.StatusCode = e switch
            {
                EduMetricsApiNoContentException => (int)HttpStatusCode.NotFound,
                EduMetricsApiNotFoundException => (int)HttpStatusCode.NotFound,
                EduMetricsApiUnauthorizedException => (int)HttpStatusCode.Unauthorized,
                EduMetricsApiInternalServerErrorException => (int)HttpStatusCode.InternalServerError,
                EduMetricsApiException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            if (response.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                _logger.LogError(e.Message + e.StackTrace);
                r = new ResponseApi(false, $"{ExceptionMessages.INTERNAL_SERVER_ERROR}. Mensagem: {e.Message}");
            }

            var json = JsonConvert.SerializeObject(r);

            await response.WriteAsync(json);
        }
    }
}