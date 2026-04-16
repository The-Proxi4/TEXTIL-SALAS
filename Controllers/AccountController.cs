using Microsoft.AspNetCore.Mvc;
using textil_salas.Models;

namespace textil_salas.Controllers
{
    public class AccountController : Controller
    {
        // LOGIN

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Login(LoginViewModel model)
{
    if (!ModelState.IsValid)
        return View(model);

    var user = FakeDatabase.Users
        .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

    if (user != null)
    {
        TempData["Usuario"] = user.Email;
        return RedirectToAction("Index", "Home");
    }

    ModelState.AddModelError("", "Correo o contraseña incorrectos.");
    return View(model);
}

        // REGISTER

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Register(RegisterViewModel model)
{
    if (!ModelState.IsValid)
        return View(model);

    // Guardar usuario en memoria
    FakeDatabase.Users.Add(new User
    {
        Email = model.Email,
        Password = model.Password
    });

    TempData["RegistroExitoso"] = "Cuenta creada correctamente. ¡Ahora ingresa!";
    return RedirectToAction("Login");
}

[HttpGet]
public IActionResult ForgotPassword()
{
    return View();
}
[HttpPost]
public IActionResult ForgotPassword(string email)
{
    var user = FakeDatabase.Users.FirstOrDefault(x => x.Email == email);

    if (user == null)
    {
        ViewBag.Error = "El correo no existe";
        return View();
    }

    // guardamos email temporal
    FakeDatabase.ResetEmail = email;

    TempData["Reset"] = "Se ha enviado un enlace de recuperación (simulado)";
    
    return RedirectToAction("ResetPassword");
}

[HttpGet]
public IActionResult ResetPassword()
{
    if (FakeDatabase.ResetEmail == null)
        return RedirectToAction("Login");

    return View();
}
[HttpPost]
public IActionResult ResetPassword(string password)
{
    var user = FakeDatabase.Users
        .FirstOrDefault(x => x.Email == FakeDatabase.ResetEmail);

    if (user != null)
    {
        user.Password = password;
    }

    FakeDatabase.ResetEmail = null;

    TempData["Success"] = "Contraseña actualizada correctamente";

    return RedirectToAction("Login");
}
    }
}