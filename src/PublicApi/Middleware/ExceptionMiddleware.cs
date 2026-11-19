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
		_logger.LogError(">>> Entering ExceptionMiddleware(next={Next})", next);
        _next = next;
		_logger.LogDebug("ExceptionMiddleware(next={Next}): _next → {_next}", next, _next);
    _logger.LogWarning("<<< Exiting ExceptionMiddleware(next={Next})", next);
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
		_logger.LogError(">>> Entering InvokeAsync(httpContext={HttpContext})", httpContext);
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
			_logger.LogError("Exception in InvokeAsync(httpContext={HttpContext}): {Arg1}", httpContext, ex.Message);
            await HandleExceptionAsync(httpContext, ex);        
        }
		_logger.LogWarning("<<< Exiting InvokeAsync(httpContext={HttpContext})", httpContext);
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
		_logger.LogError(">>> Entering HandleExceptionAsync(context={Context},exception={Exception})", context, exception);
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
		_logger.LogWarning("<<< Exiting HandleExceptionAsync(context={Context},exception={Exception})", context, exception);
    }
}
