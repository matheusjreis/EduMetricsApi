using EduMetricsApi.Domain.Constants;

namespace EduMetricsApi.Application.Exceptions;

public class EduMetricsApiException : Exception
{
    public EduMetricsApiException(string message) : base(message) { }

    public class EduMetricsApiNoContentException : EduMetricsApiException
    {
        public EduMetricsApiNoContentException() : base(ExceptionMessages.NO_CONTENT) { }
    }

    public class EduMetricsApiNotFoundException : EduMetricsApiException
    {
        public EduMetricsApiNotFoundException() : base(ExceptionMessages.NOT_FOUND) { }
    }

    public class EduMetricsApiInternalServerErrorException : EduMetricsApiException
    {
        public EduMetricsApiInternalServerErrorException() : base(ExceptionMessages.INTERNAL_SERVER_ERROR) { }
    }

    public class EduMetricsApiUnauthorizedException : EduMetricsApiException
    {
        public EduMetricsApiUnauthorizedException() : base(ExceptionMessages.UNAUTHORIZED) { }
    }
}
