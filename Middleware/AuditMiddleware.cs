// Handle things like logging, authentication, exception handling, rate limiting.
// Example: Audit middleware to log every change or request details.
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Diagnostics;
using System.Threading.Tasks;

namespace InvoiceManagement1.Middleware
{
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next;

        public AuditMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();

            Log.Information("AUDIT: {Method} {Path} executed in {Duration}ms with status {StatusCode}",
                context.Request.Method,
                context.Request.Path,
                stopwatch.ElapsedMilliseconds,
                context.Response.StatusCode);
        }
    }
}
