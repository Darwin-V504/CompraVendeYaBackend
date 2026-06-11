using System.Net;
using System.Text.Json;

namespace CompraVendeYaBackend.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, exception.Message),
            KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
            InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
            // Errores de negocio conocidos que usan Exception genérica
            Exception e when e.Message.Contains("incorrectos", StringComparison.OrdinalIgnoreCase)
                => (HttpStatusCode.Unauthorized, e.Message),
            Exception e when e.Message.Contains("ya está registrado", StringComparison.OrdinalIgnoreCase)
                => (HttpStatusCode.Conflict, e.Message),
            _ => (HttpStatusCode.InternalServerError, "Ha ocurrido un error inesperado.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var body = JsonSerializer.Serialize(new { message });
        return context.Response.WriteAsync(body);
    }
}
