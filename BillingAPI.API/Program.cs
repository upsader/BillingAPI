using BillingAPI.Core.Interfaces;
using BillingAPI.Services.PaymentGateways;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add HttpClient for payment gateways
builder.Services.AddHttpClient();

// Add application services
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddSingleton<IPaymentGatewayFactory, PaymentGatewayFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();