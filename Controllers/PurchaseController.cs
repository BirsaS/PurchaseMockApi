using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/purchase-requests")]
public class PurchaseController : ControllerBase
{
    private static bool _isApiDown = false;

    [HttpPost]
    public IActionResult CreatePurchaseRequest([FromBody] PurchaseRequestDto request)
    {
        if (_isApiDown)
        {
            return StatusCode(503, new { error = "Zunanji ERP sistem je trenutno nedosegljiv." });
        }

        if (string.IsNullOrEmpty(request.Title) || request.Amount <= 0)
        {
            return BadRequest(new { error = "Manjkajo obvezni podatki." });
        }

        var random = new Random();
        string orderId = $"ORD-{random.Next(10000, 99999)}";

        return Ok(new
        {
            success = true,
            orderId = orderId,
            message = "Zahtevek je bil uspešno sprejet v ERP."
        });
    }

    [HttpPost("toggle-status")]
    public IActionResult ToggleApiStatus([FromQuery] bool down)
    {
        _isApiDown = down;
        return Ok(new { apiDown = _isApiDown });
    }
}

public class PurchaseRequestDto
{
    public string Title { get; set; }
    public decimal Amount { get; set; }
    public string Department { get; set; }
    public string ProposedSupplier { get; set; }
    public DateTime DesiredDeliveryDate { get; set; }
}