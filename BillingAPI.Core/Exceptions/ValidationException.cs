namespace BillingAPI.Core.Exceptions
{
    public class ValidationException : BillingException
    {
        public ValidationException(string message) : base(message) { }
    }
}
