using Microsoft.AspNetCore.Mvc;

namespace Optivem.EShop.Monolith.Controllers;

[ApiController]
public class EchoController : ControllerBase
{
    [HttpGet("/api/echo")]
    public IActionResult Echo()
    {
        return Ok();
    }
}
