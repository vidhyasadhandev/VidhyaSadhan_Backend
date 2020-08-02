using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Helpers
{
    public class PreflightRequests
    {
        private readonly RequestDelegate Next;
        public PreflightRequests(RequestDelegate next)
        {
            Next = next;
        }

        public Task Invoke(HttpContext context)
        {
            return BeginInvoke(context);
        }

        private Task BeginInvoke(HttpContext context)
        {
            return Next.Invoke(context);
        }
    }

    public static class PreflightRequestExtensions
    {
        public static IApplicationBuilder UsePreflightRequestHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PreflightRequests>();
        }
    }
}
