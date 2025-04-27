using BillingAPI.Core.Interfaces;

namespace BillingAPI.Services.PaymentGateways
{
    public class PaymentGatewayFactory : IPaymentGatewayFactory
    {
        private readonly Dictionary<string, IPaymentGateway> _gateways = [];

        public IPaymentGateway GetGateway(string gatewayId)
        {
            if (string.IsNullOrWhiteSpace(gatewayId))
            {
                throw new ArgumentException("Gateway ID cannot be empty", nameof(gatewayId));
            }

            if (_gateways.TryGetValue(gatewayId, out var gateway))
            {
                return gateway;
            }

            throw new ArgumentException($"No payment gateway registered for ID: {gatewayId}", nameof(gatewayId));
        }

        public void RegisterGateway(string gatewayId, IPaymentGateway gateway)
        {
            if (string.IsNullOrWhiteSpace(gatewayId))
            {
                throw new ArgumentException("Gateway ID cannot be empty", nameof(gatewayId));
            }

            if (gateway == null)
            {
                throw new ArgumentNullException(nameof(gateway));
            }

            _gateways[gatewayId] = gateway;
        }
    }
}
