using BillingAPI.Core.Exceptions;
using BillingAPI.Core.Interfaces;
using BillingAPI.Core.Models;
using Microsoft.Extensions.Logging;

public class BillingService : IBillingService
{
    private readonly IPaymentGatewayFactory _paymentGatewayFactory;
    private readonly IReceiptRepository _receiptRepository;
    private readonly ILogger<BillingService> _logger;

    public BillingService(IPaymentGatewayFactory paymentGatewayFactory, IReceiptRepository receiptRepository, ILogger<BillingService> logger)
    {
        _paymentGatewayFactory = paymentGatewayFactory;
        _receiptRepository = receiptRepository;
        _logger = logger;
    }

    public async Task<Receipt> ProcessOrderAsync(OrderRequest order)
    {
        _logger.LogInformation("Processing order {OrderNumber}", order.OrderNumber);

        ValidateOrder(order);
        var paymentGateway = _paymentGatewayFactory.GetGateway(order.PaymentGateway);

        var paymentResult = await paymentGateway.ProcessPaymentAsync(order);

        if (!paymentResult.IsSuccess)
        {
            throw new PaymentProcessingException(paymentResult.Message ?? $"Payment failed for order {order.OrderNumber}");
        }

        var receipt = new Receipt
        {
            Id = Guid.NewGuid().ToString(),
            OrderNumber = order.OrderNumber,
            UserId = order.UserId,
            Amount = order.PayableAmount,
            PaymentGateway = order.PaymentGateway,
            ProcessedDate = DateTime.UtcNow,
            TransactionId = paymentResult.TransactionId,
            Description = order.Description
        };

        if (await _receiptRepository.OrderNumberExistsAsync(order.OrderNumber))
        {
            throw new ValidationException($"Order number {order.OrderNumber} already exists");
        }

        await _receiptRepository.AddAsync(receipt);
        await _receiptRepository.SaveChangesAsync();

        return receipt;
    }

    public async Task<Receipt> GetReceiptAsync(string orderNumber)
    {
        var receipt = await _receiptRepository.GetByOrderNumberAsync(orderNumber);

        if (receipt == null)
        {
            _logger.LogWarning("Receipt not found for order {OrderNumber}", orderNumber);
            throw new NotFoundException($"Receipt for order {orderNumber} not found");
        }

        return receipt;
    }

    private void ValidateOrder(OrderRequest order)
    {
        if (order == null)
        {
            throw new ArgumentNullException(nameof(order));
        }

        if (string.IsNullOrWhiteSpace(order.OrderNumber))
        {
            throw new ValidationException("Order number is required");

        }

        if (string.IsNullOrWhiteSpace(order.UserId))
        {
            throw new ValidationException("User ID is required");

        }

        if (order.PayableAmount <= 0)
        {
            throw new ValidationException("Payable amount must be greater than zero");

        }

        if (string.IsNullOrWhiteSpace(order.PaymentGateway))
        {
            throw new ValidationException("Payment gateway is required");
        }
    }
}