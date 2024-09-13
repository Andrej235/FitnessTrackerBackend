using Microsoft.AspNetCore.Diagnostics;

namespace FitnessTracker.Exceptions
{
    public class ExceptionHandlerMiddleware : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            switch (exception)
            {
                case BadRequestException: // 400
                    httpContext.Response.StatusCode = 400;
                    break;

                case NotFoundException: // 404
                    httpContext.Response.StatusCode = 404;
                    break;

                case AccessDeniedException: // 403
                    httpContext.Response.StatusCode = 403;
                    break;

                case UnauthorizedException: // 401
                    httpContext.Response.StatusCode = 401;
                    break;

                default:
                    return false;
            }

            await httpContext.Response.WriteAsync(exception.Message, cancellationToken);
            return true;
        }
    }
}
