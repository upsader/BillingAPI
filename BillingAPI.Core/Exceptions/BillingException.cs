namespace BillingAPI.Core.Exceptions
{
    public abstract class BillingException : Exception
    {
        protected BillingException(string message) : base(message) { }
    }
}
