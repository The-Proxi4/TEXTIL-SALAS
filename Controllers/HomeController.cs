using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using textil_salas.Models;

namespace textil_salas.Controllers;


public class HomeController : Controller
{

    [Route("tienda")]
// En HomeController, método Shop():
public IActionResult Shop(string? q, string? category)
{
    // CA-5: Solo productos de categorías activas
    var categoriasActivas = FakeDatabase.Categorias
        .Where(c => c.Activo)
        .Select(c => c.Nombre)
        .ToHashSet(StringComparer.OrdinalIgnoreCase);

    var productos = FakeDatabase.Products
        .Where(p => categoriasActivas.Contains(p.Category))
        .ToList();

    // Filtros existentes...
    if (!string.IsNullOrEmpty(q))
        productos = productos.Where(p => p.Name.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();

if (!string.IsNullOrEmpty(category) && category != "all")
{
    productos = productos
        .Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
        .ToList();
}

    ViewBag.Category = category ?? "all";
    ViewBag.Query = q ?? string.Empty;
    ViewBag.CategoriasActivas = FakeDatabase.Categorias.Where(c => c.Activo).ToList();

    return View(productos);
}

    [HttpGet]
    [Route("b2b")]
    public IActionResult B2B() => View(new B2BQuoteViewModel());

    [HttpPost]
    [Route("b2b")]
    [ValidateAntiForgeryToken]
    public IActionResult B2B(B2BQuoteViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        TempData["Success"] = "Solicitud enviada. El equipo B2B te contactará en 24 horas.";
        return RedirectToAction(nameof(B2B));
    }

    [Route("mis-pedidos")]
    public IActionResult Orders()
    {
        var orders = new List<OrderSummary>
        {
            new() { Id = 1045, Customer = "Corporación Salas", Total = 1550m, Status = "Pendiente", CreatedAt = DateTime.Today.AddDays(-1) },
            new() { Id = 1040, Customer = "Hotel Sol Andino", Total = 3200m, Status = "Entregado", CreatedAt = DateTime.Today.AddDays(-6) }
        };
        return View(orders);
    }

public IActionResult Index()
{
    return View(FakeDatabase.Products.Take(4).ToList());
}
}
