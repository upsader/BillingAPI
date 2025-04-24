namespace BillingAPI.Core.Exceptions
{
    public class NotFoundException : BillingException
    {
        public NotFoundException(string message) : base(message) { }
    }
}
