using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using textil_salas.Models;
using textil_salas.ViewModels;

namespace textil_salas.Controllers.Admin;

[Authorize]
[Route("admin/clients")]
public class ClientsController : Controller
{
    private readonly ApplicationDbContext _db;

    public ClientsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(string q = null, string email = null, string status = "all")
    {
        var users = _db.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
            users = users.Where(u => u.NombreCompleto.Contains(q) || u.UserName.Contains(q));
        if (!string.IsNullOrWhiteSpace(email))
            users = users.Where(u => u.Email.Contains(email));
        if (status == "active")
            users = users.Where(u => u.IsActive);
        else if (status == "inactive")
            users = users.Where(u => !u.IsActive);

        var list = await users.OrderByDescending(u => u.Id).Take(200).ToListAsync();
        var vm = list.Select(ClientViewModel.FromUser).ToList();
        return View("~/Views/Admin/Clients/Index.cshtml", vm);
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();
        return View("~/Views/Admin/Clients/Edit.cshtml", ClientViewModel.FromUser(user));
    }

    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, ClientViewModel model)
    {
        if (!ModelState.IsValid) return View("~/Views/Admin/Clients/Edit.cshtml", model);
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();

        // basic unique email check
        var exists = await _db.Users.AnyAsync(u => u.Email == model.Email && u.Id != id);
        if (exists)
        {
            ModelState.AddModelError("Email", "El correo ya está en uso por otro usuario.");
            return View("~/Views/Admin/Clients/Edit.cshtml", model);
        }

        user.NombreCompleto = model.NombreCompleto;
        user.Email = model.Email;
        user.UserName = model.Email;
        user.PhoneNumber = model.PhoneNumber;
        user.IsActive = model.IsActive;
        user.DeactivatedAt = model.IsActive ? null : (model.DeactivatedAt ?? DateTime.UtcNow);

        _db.Users.Update(user);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Cliente actualizado correctamente.";
        return RedirectToAction("Index");
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(string id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();
        return View("~/Views/Admin/Clients/Details.cshtml", ClientViewModel.FromUser(user));
    }

    [HttpPost("toggle/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(string id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();
        user.IsActive = !user.IsActive;
        user.DeactivatedAt = user.IsActive ? null : DateTime.UtcNow;
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
        TempData["Success"] = user.IsActive ? "Cliente reactivado." : "Cliente desactivado.";
        return RedirectToAction("Index");
    }
}
