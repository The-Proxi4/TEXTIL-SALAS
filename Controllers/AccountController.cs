using System.Linq;
using Microsoft.AspNetCore.Mvc;
using textil_salas.Models;

namespace textil_salas.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (TryAuthenticate(model.Email, model.Password, out var user))
            {
                TempData["Usuario"] = user.Email;
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            FakeDatabase.Instance.Users.Add(new User
            {
                Email = model.Email,
                Password = model.Password
            });

            TempData["RegistroExitoso"] = "Cuenta creada correctamente. ¡Ahora ingresa!";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError(string.Empty, "Debes ingresar un correo.");
                return View();
            }

            var user = GetUserByEmail(email);
            if (user == null)
            {
                ViewBag.Error = "El correo no existe";
                return View();
            }

            FakeDatabase.Instance.ResetEmail = email;
            TempData["Reset"] = "Se ha enviado un enlace de recuperación (simulado)";
            return RedirectToAction(nameof(ResetPassword));
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            if (string.IsNullOrWhiteSpace(FakeDatabase.Instance.ResetEmail))
                return RedirectToAction(nameof(Login));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "Debes ingresar una contraseña.");
                return View();
            }

            var user = GetUserByEmail(FakeDatabase.Instance.ResetEmail);
            if (user != null)
                user.Password = password;

            FakeDatabase.Instance.ResetEmail = null;
            TempData["Success"] = "Contraseña actualizada correctamente";
            return RedirectToAction(nameof(Login));
        }

        private bool TryAuthenticate(string email, string password, out User user)
        {
            user = FakeDatabase.Instance.Users
                .FirstOrDefault(u => u.Email == email && u.Password == password);
            return user != null;
        }

        private User GetUserByEmail(string email)
        {
            return FakeDatabase.Instance.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
