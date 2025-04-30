using BillingAPI.Core.Exceptions;
using BillingAPI.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace BillingAPI.Services.PaymentGateways
{
    public class PaymentGatewayFactory : IPaymentGatewayFactory
    {
        private readonly Dictionary<string, IPaymentGateway> _gateways = [];
        private readonly ILogger<PaymentGatewayFactory> _logger;

        public PaymentGatewayFactory(ILogger<PaymentGatewayFactory> logger)
        {
            _logger = logger;
        }

        public IPaymentGateway GetGateway(string gatewayId)
        {
            if (!_gateways.TryGetValue(gatewayId, out var gateway))
            {
                _logger.LogWarning("No payment gateway registered for ID: {GatewayId}", gatewayId);
                throw new NotFoundException($"No payment gateway registered for ID: {gatewayId}");
            }

            _logger.LogInformation("Successfully retrieved payment gateway for ID: {GatewayId}", gatewayId);
            return gateway;
        }

        public void RegisterGateway(string gatewayId, IPaymentGateway gateway)
        {
            _gateways[gatewayId] = gateway;
            _logger.LogInformation("Successfully registered payment gateway for ID: {GatewayId}", gatewayId);
        }
    }
}
