using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MiddlewareDemo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder =>
            builder.AddConsole()
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Starting ...");
            app.Map("/newbranch", a => {
                a.Map("/branch1", brancha => brancha
                    .Run(c => c.Response.WriteAsync("Running from the newbranch/branch1 branch!")));
                a.Map("/branch2", brancha => brancha
                    .Run(c => c.Response.WriteAsync("Running from the newbranch/branch2 branch!")));

                a.Run(c => c.Response.WriteAsync("Running from the newbranch branch!"));
            });


            app.Use(async (context, next) =>
            {
                logger.LogInformation("Start in Use Middleware");
                await next();
                logger.LogInformation("End in Use Middleware");
            });
            //   app.UseMiddleware<CustomMiddleware>();
            app.AAAA();
            app.Map("/employees", a => a.Run(async context =>
             {
                 await context.Response.WriteAsync("List of Employees");
             }));
            app.MapWhen(context => context.Request.Path.Value == "testing",
                a => a.Run(async context =>
                  {
                      await context.Response.WriteAsync("Hello from Testing");
                  }));
          //  app.MapWhen(checkRequest, provideResponse);
            app.UseStaticFiles();
            app.Use(async (context,next)=>
            {
                await context.Response.WriteAsync("Hello from 1st block");
                await next();
            });
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello from 2nd block");
            });
        }

        private void provideResponse(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("Hello from Testing");
            });
        }

        private bool checkRequest(HttpContext arg)
        {
            if (arg.Request.Path.Value == "/testing")
                return true;
            else
                return false;
        }
    }


}