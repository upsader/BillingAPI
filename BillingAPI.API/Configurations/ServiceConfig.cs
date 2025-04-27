using BillingAPI.Core.Interfaces;
using BillingAPI.Infrastructure.Data;
using BillingAPI.Infrastructure.Repositories;
using BillingAPI.Services.PaymentGateways;
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.API.Configurations
{
    public static class ServiceConfig
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            ConfigureDatabase(services, configuration);
            ConfigureApplicationServices(services, env);

            return services;
        }

        private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BillingDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured."),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(3);
                        sqlOptions.CommandTimeout(30);
                    }
                );
            });
        }

        private static void ConfigureApplicationServices(IServiceCollection services, IWebHostEnvironment env)
        {
            services.AddScoped<IReceiptRepository, ReceiptRepository>();
            services.AddScoped<IBillingService, BillingService>();

            // In development environment
            if (env.IsDevelopment())
            {
                var mockFactory = new MockPaymentGatewayFactory();
                mockFactory.InitializeDefaultGateways();
                services.AddSingleton<IPaymentGatewayFactory>(mockFactory);
            }
            else
            {
                services.AddSingleton<IPaymentGatewayFactory, PaymentGatewayFactory>();
            }

            services.AddMemoryCache();
        }
    }
}