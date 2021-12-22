using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddlewareDemo
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _MyNext;
        private readonly IHostingEnvironment _MyEnv;
        private ILogger _logger;
        public CustomMiddleware(RequestDelegate next, IHostingEnvironment env, ILogger<Startup> logger)
        {
            _MyNext = next;
            _MyEnv = env;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("Hello from Custon Middleware");
            await _MyNext(context);
            _logger.LogInformation("Bye from Custom Middleware");

        }
    }
    public static class Wrapper
    {
        public static IApplicationBuilder AAAA(this IApplicationBuilder app)
        {

            return app.UseMiddleware<CustomMiddleware>();
        }

    }
}