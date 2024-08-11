namespace EduMetricsApi.Infra.Data.Extensions;

public static class ObjectExtension
{
    public static string NullToString(this object? obj)
    {
        return obj == null ? "" : obj.ToString()!;
    }
}