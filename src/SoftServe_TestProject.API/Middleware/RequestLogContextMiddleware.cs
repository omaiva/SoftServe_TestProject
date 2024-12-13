using Serilog;
using Serilog.Context;

namespace SoftServe_TestProject.API.Middleware
{
    public class RequestLogContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;

            Log.Information("Incoming request: {Method} {Path} {QueryString}",
                request.Method, request.Path, request.QueryString);

            await _next(context);

            Log.Information("Outgoing response: {StatusCode}", context.Response.StatusCode);
        }
    }
}
