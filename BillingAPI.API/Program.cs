using BillingAPI.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

// Services
builder.Services
    .ConfigureServices(builder.Configuration, builder.Environment)
    .ConfigureSwagger();

var app = builder.Build();

// Middleware
app.ConfigureMiddleware(builder.Environment);

app.Run();