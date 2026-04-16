using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using textil_salas.Models;

namespace textil_salas.Controllers;


public class HomeController : Controller
{
    public IActionResult Index() => View(GetProducts().Take(4).ToList());

    [Route("tienda")]
    public IActionResult Shop(string? category, string? q)
    {
        var products = GetProducts().AsEnumerable();
        if (!string.IsNullOrWhiteSpace(category) && category != "all")
            products = products.Where(x => x.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(q))
            products = products.Where(x => x.Name.Contains(q, StringComparison.OrdinalIgnoreCase) || x.Description.Contains(q, StringComparison.OrdinalIgnoreCase));
        ViewBag.Category = category ?? "all";
        ViewBag.Query = q ?? string.Empty;
        return View(products.ToList());
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

    private static List<Product> GetProducts() =>
    [
        new() { Id = "towel-hand-white", Name = "Toalla de Mano Blanca", Description = "Algodón prima 700 GSM, ideal para baños de hotel y uso diario.", Price = 70, Category = "towel", Subcategory = "Mano", ImageUrl = "/images/towel-hand.jpg", Material = "100% Algodón Prima", Gsm = 700, Sizes = ["40x70cm", "50x80cm"], B2BMinQty = 100 },
        new() { Id = "towel-floor-white", Name = "Toalla de Piso Blanca", Description = "Alfombra de baño antideslizante de algodón prima 750 GSM.", Price = 85, Category = "towel", Subcategory = "Piso", ImageUrl = "/images/towel-floor.jpg", Material = "100% Algodón Prima", Gsm = 750, Sizes = ["50x70cm", "60x90cm"], B2BMinQty = 100 },
        new() { Id = "towel-bath-white", Name = "Toalla de Baño Blanca", Description = "Toalla de cuerpo completo en algodón prima 750 GSM.", Price = 95, Category = "towel", Subcategory = "Baño", ImageUrl = "/images/towel-white.jpg", Material = "100% Algodón Prima", Gsm = 750, Sizes = ["70x140cm", "80x160cm"], B2BMinQty = 50 },
        new() { Id = "robe-white", Name = "Bata de Baño Blanca", Description = "Bata terry cloth premium de algodón prima 700 GSM.", Price = 100, Category = "robe", Subcategory = "Bata", ImageUrl = "/images/robe-white.jpg", Material = "100% Algodón Prima", Gsm = 700, Sizes = ["S", "M", "L", "XL"], B2BMinQty = 25 },
    ];
}
