namespace EduMetricsApi.Application.Helpers;

public class ResponseApi
{
    public bool success { get; set; }
    public object? data { get; set; } = default;
    public string? message { get; set; } = null;

    public ResponseApi(bool successResponse, string? messageResponse = null, object? dataResponse = null)
    {
        success = successResponse;
        data = dataResponse;
        message = messageResponse;
    }
}