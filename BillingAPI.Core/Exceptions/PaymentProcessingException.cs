namespace BillingAPI.Core.Exceptions
{
    public class PaymentProcessingException : BillingException
    {
        public PaymentProcessingException(string message) : base(message) { }
    }
}
