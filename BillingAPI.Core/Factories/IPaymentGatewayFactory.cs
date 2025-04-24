namespace BillingAPI.Core.Interfaces
{
    public interface IPaymentGatewayFactory
    {
        IPaymentGateway GetGateway(string gatewayId);
        void RegisterGateway(string gatewayId, IPaymentGateway gateway);
    }
}