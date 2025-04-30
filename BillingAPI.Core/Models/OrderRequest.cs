namespace BillingAPI.Core.Models
{
    public record OrderRequest(
        string OrderNumber,
        string UserId,
        decimal PayableAmount,
        string PaymentGateway,
        string? Description
    );
}
