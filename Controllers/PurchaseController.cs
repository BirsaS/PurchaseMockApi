using Microsoft.AspNetCore.Mvc;

namespace PurchaseMockApi.Controllers
{
    [ApiController]
    [Route("api/purchase-requests")]
    public class PurchaseRequestsController : ControllerBase
    {
        private static bool _isSystemDown = false;

        [HttpPost]
        public IActionResult CreateRequest([FromBody] PurchaseRequest request)
        {
            if (_isSystemDown)
            {
                return StatusCode(503, new { message = "Service Unavailable (Simulated failure via toggle-status)" });
            }

            return Ok(new
            {
                Message = "Zahtevek uspešno sprejet!",
                RequestId = Guid.NewGuid(),
                Data = request,
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpPost("toggle-status")]
        public IActionResult ToggleStatus()
        {
            _isSystemDown = !_isSystemDown;
            var statusText = _isSystemDown ? "Sistem sedaj vrača napako 503 (DOWN)" : "Sistem deluje normalno (UP)";
            return Ok(new { SystemStatus = statusText, IsDown = _isSystemDown });
        }
    }

    public record PurchaseRequest(
        string Title,
        decimal Amount,
        string Department,
        string ProposedSupplier,
        DateTime DesiredDeliveryDate
    );
}