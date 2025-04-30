using BillingAPI.Core.Interfaces;
using BillingAPI.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BillingAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IBillingService _billingService;
        private readonly ILogger<BillingController> _logger;

        public BillingController(IBillingService billingService, ILogger<BillingController> logger)
        {
            _billingService = billingService;
            _logger = logger;
        }

        [HttpPost("process-order")]
        public async Task<ActionResult<Receipt>> ProcessOrder([FromBody] OrderRequest request)
        {
            _logger.LogInformation("Processing order {OrderNumber} for user {UserId}", request.OrderNumber, request.UserId);
            var receipt = await _billingService.ProcessOrderAsync(request);
            _logger.LogInformation("Order {OrderNumber} processed successfully", request.OrderNumber);
            return Ok(receipt);
        }

        [HttpGet("receipts/{orderNumber}")]
        public async Task<ActionResult<Receipt>> GetReceipt(string orderNumber)
        {
            _logger.LogInformation("Retrieving receipt for order {OrderNumber}", orderNumber);
            var receipt = await _billingService.GetReceiptAsync(orderNumber);
            _logger.LogInformation("Receipt for order {OrderNumber} retrieved successfully", orderNumber);
            return Ok(receipt);
        }
    }
}
