using BillingAPI.Core.Exceptions;
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

        public BillingController(IBillingService billingService)
        {
            _billingService = billingService;
        }

        [HttpPost("process-order")]
        public async Task<ActionResult<Receipt>> ProcessOrder([FromBody] OrderRequest request)
        {
            try
            {
                var receipt = await _billingService.ProcessOrderAsync(request);
                return Ok(receipt);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (PaymentProcessingException ex)
            {
                return StatusCode(402, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("receipts/{orderNumber}")]
        public async Task<ActionResult<Receipt>> GetReceipt(string orderNumber)
        {
            try
            {
                var receipt = await _billingService.GetReceiptAsync(orderNumber);
                return Ok(receipt);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}