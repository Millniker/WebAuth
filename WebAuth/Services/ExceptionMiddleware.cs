using Pathnostics.Web.Models.DTO;
using university.Server.Exception.ExceptionsModels;

namespace WebAuth.Services;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (IncorrectDataException e)
        {
            _logger.LogError(e.Message);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ErrorResponse() { Code = "400", Message = e.Message });
        }
        catch (ItemNotFoundException e)
        {
            _logger.LogError(e.Message);
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new ErrorResponse { Code = "404", Message = e.Message });
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new ErrorResponse { Code = "500", Message = e.Message });
        }
    }
}