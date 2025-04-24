namespace BillingAPI.Core.Models
{
    public class Receipt
    {
        public string Id { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentGateway { get; set; }
        public DateTime ProcessedDate { get; set; }
        public string TransactionId { get; set; }
        public string? Description { get; set; }
    }
}
