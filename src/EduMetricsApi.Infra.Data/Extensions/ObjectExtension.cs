namespace EduMetricsApi.Infra.Data.Extensions;

public static class ObjectExtension
{
    public static string NullToString(this object? obj)
    {
        return obj == null ? "" : obj.ToString()!;
    }
    public static string GetBrowserName(string userAgent)
    {
        string browser;

        if (userAgent.Contains("Opera") || userAgent.Contains("Opr"))
        {
            browser = "Opera";
        }
        else if (userAgent.Contains("Edg"))
        {
            browser = "Edge";
        }
        else if (userAgent.Contains("Chrome"))
        {
            browser = "Chrome";
        }
        else if (userAgent.Contains("Safari"))
        {
            browser = "Safari";
        }
        else if (userAgent.Contains("Firefox"))
        {
            browser = "Firefox";
        }
        else
        {
            browser = "unknown";
        }

        return browser;
    }

}