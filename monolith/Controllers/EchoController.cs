using Microsoft.AspNetCore.Mvc;

namespace Optivem.AtddAccelerator.EShop.Monolith.Controllers;

[ApiController]
public class EchoController : ControllerBase
{
    [HttpGet("/api/echo")]
    public IActionResult Echo()
    {
        return Ok();
    }
}
