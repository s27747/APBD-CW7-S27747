namespace MiniHelpdesk.Web.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled application error");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "text/html; charset=utf-8";

            await context.Response.WriteAsync("""
                                              <html>
                                                  <head>
                                                      <title>Błąd aplikacji</title>
                                                  </head>
                                                  <body>
                                                      <h1>Wystąpił błąd aplikacji</h1>
                                                      <p>Spróbuj ponownie później.</p>
                                                  </body>
                                              </html>
                                              """);
        }
    }
}