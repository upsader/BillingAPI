using BillingAPI.API.Models;
using BillingAPI.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace BillingAPI.API.Configurations
{
    public static class MiddlewareConfig
    {
        public static WebApplication ConfigureMiddleware(this WebApplication app, IWebHostEnvironment env)
        {
            // Exception Handling
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (exceptionHandlerFeature != null)
                    {
                        var exception = exceptionHandlerFeature.Error;

                        context.Response.ContentType = "application/json";

                        var statusCode = exception switch
                        {
                            ArgumentNullException => StatusCodes.Status400BadRequest,
                            ValidationException => StatusCodes.Status400BadRequest,
                            NotFoundException => StatusCodes.Status404NotFound,
                            PaymentProcessingException => StatusCodes.Status402PaymentRequired,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        context.Response.StatusCode = statusCode;

                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (contextFeature != null)
                        {
                            await context.Response.WriteAsync(new ErrorDetails
                            {
                                StatusCode = statusCode,
                                Message = exception.Message
                            }.ToString());
                        }
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