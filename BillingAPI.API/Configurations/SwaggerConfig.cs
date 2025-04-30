using Microsoft.OpenApi.Models;

namespace BillingAPI.API.Configurations
{
    public static class SwaggerConfig
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Billing API",
                    Version = "v1",
                    Description = "API for managing billing and payments",
                    Contact = new OpenApiContact
                    {
                        Name = "Olegs",
                        Email = "o.kuzicevs@gmail.com"
                    }
                });
            });

            return services;
        }
    }
}