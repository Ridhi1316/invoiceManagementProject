using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace InvoiceManagement1.Middleware
{
    public class RateLimitingMiddleware
    {
        private static ConcurrentDictionary<string, int> RequestCounts = new();
        private readonly RequestDelegate _next;

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress.ToString();
            RequestCounts.TryGetValue(ip, out int count);

            if (count > 100)
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsync("Too many requests.");
                return;
            }

            RequestCounts[ip] = count + 1;
            await _next(context);
        }
    }
}
