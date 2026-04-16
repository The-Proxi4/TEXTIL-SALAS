using Microsoft.AspNetCore.Mvc;

namespace TextilSalas.Mvc.Controllers;

[Route("auth")]
public class AccountController : Controller
{
    [HttpGet("")]
    [HttpGet("login")]
    public IActionResult Login() => View();
}
