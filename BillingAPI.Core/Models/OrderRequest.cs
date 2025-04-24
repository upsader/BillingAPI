namespace BillingAPI.Core.Models
{
    public class OrderRequest
    {
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public decimal PayableAmount { get; set; }
        public string PaymentGateway { get; set; }
        public string? Description { get; set; }
    }
}
