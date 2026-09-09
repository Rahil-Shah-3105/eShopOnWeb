using System;
using System.Net;
using System.Threading.Tasks;
using BlazorShared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.eShopWeb.ApplicationCore.Exceptions;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi.Middleware;

public class ExceptionMiddleware
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
		_logger.LogInformation("ENTERING: ExceptionMiddleware() method");
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
		_logger.LogInformation("ENTERING: InvokeAsync() method");
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);        
        }
		_logger.LogWarning("InvokeAsync(httpContext)={HttpContext}): GDPR-AUDIT: Outbound notification recorded for the audit trail", httpContext);
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
		_logger.LogInformation("ENTERING: HandleExceptionAsync() method");
        context.Response.ContentType = "application/json";

        if (exception is DuplicateException duplicationException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = duplicationException.Message
            }.ToString());
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }
		_logger.LogWarning("HandleExceptionAsync(context,exception)={Context},{Exception}: GDPR-AUDIT: Outbound notification recorded for the audit trail", context, exception);
    }
}
