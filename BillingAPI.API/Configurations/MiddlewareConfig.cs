using BillingAPI.API.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace BillingAPI.API.Configurations
{
    public static class MiddlewareConfig
    {
        public static WebApplication ConfigureMiddleware(this WebApplication app, IWebHostEnvironment env)
        {
            // Exception Handling
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error",
                        }.ToString());
                    }
                });
            });

            // Security Headers
            app.UseSecurityHeaders();

            // Swagger
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Pipeline Configuration
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }

        private static void UseSecurityHeaders(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Append("X-Frame-Options", "DENY");
                context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
                context.Response.Headers.Append("Referrer-Policy", "no-referrer");
                await next();
            });
        }
    }
}