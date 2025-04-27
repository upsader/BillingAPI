using BillingAPI.Core.Interfaces;

namespace BillingAPI.Services.PaymentGateways
{
    public class MockPaymentGatewayFactory : IPaymentGatewayFactory
    {
        private readonly Dictionary<string, IPaymentGateway> _gateways = [];

        public IPaymentGateway GetGateway(string gatewayId)
        {
            if (string.IsNullOrWhiteSpace(gatewayId))
            {
                throw new ArgumentException("Gateway ID cannot be empty", nameof(gatewayId));
            }

            if (!_gateways.TryGetValue(gatewayId, out var gateway))
            {
                throw new ArgumentException($"No payment gateway registered for ID: {gatewayId}");
            }

            return gateway;
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

        // Initialize with default mock gateways
        public void InitializeDefaultGateways()
        {
            RegisterGateway("Stripe", new MockStripeGateway());
            RegisterGateway("PayPal", new MockPayPalGateway());
        }
    }
}