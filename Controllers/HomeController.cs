using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using textil_salas.Models;

namespace textil_salas.Controllers;

public class HomeController : Controller
{
    [HttpGet("tienda")]
    public IActionResult Shop(string? q, string? category)
    {
        var productos = GetActiveProducts();
        productos = FilterProductsByQuery(productos, q).ToList();
        productos = FilterProductsByCategory(productos, category).ToList();

        ViewBag.Category = category ?? "all";
        ViewBag.Query = q ?? string.Empty;
        ViewBag.CategoriasActivas = GetActiveCategories();

        return View(productos);
    }

    [HttpGet("b2b")]
    public IActionResult B2B() => View(new B2BQuoteViewModel());

    [HttpPost("b2b")]
    [ValidateAntiForgeryToken]
    public IActionResult B2B(B2BQuoteViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        TempData["Success"] = "Solicitud enviada. El equipo B2B te contactará en 24 horas.";
        return RedirectToAction(nameof(B2B));
    }

    [HttpGet("mis-pedidos")]
    public IActionResult Orders()
    {
        var orders = CreateSampleOrders();
        return View(orders);
    }

    public IActionResult Index()
    {
        return View(GetFeaturedProducts());
    }

    private static IEnumerable<Product> GetActiveProducts()
    {
        var categoriasActivas = FakeDatabase.Instance.Categorias
            .Where(c => c.Activo)
            .Select(c => c.Nombre)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        return FakeDatabase.Instance.Products
            .Where(p => categoriasActivas.Contains(p.Category));
    }

    private static IEnumerable<Product> FilterProductsByQuery(IEnumerable<Product> products, string? query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return products;

        return products.Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase));
    }

    private static IEnumerable<Product> FilterProductsByCategory(IEnumerable<Product> products, string? category)
    {
        if (string.IsNullOrWhiteSpace(category) || category.Equals("all", StringComparison.OrdinalIgnoreCase))
            return products;

        return products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
    }

    private static List<Categoria> GetActiveCategories()
    {
        return FakeDatabase.Instance.Categorias
            .Where(c => c.Activo)
            .ToList();
    }

    private static List<OrderSummary> CreateSampleOrders()
    {
        return new()
        {
            new() { Id = 1045, Customer = "Corporación Salas", Total = 1550m, Status = "Pendiente", CreatedAt = DateTime.Today.AddDays(-1) },
            new() { Id = 1040, Customer = "Hotel Sol Andino", Total = 3200m, Status = "Entregado", CreatedAt = DateTime.Today.AddDays(-6) }
        };
    }

    private static List<Product> GetFeaturedProducts()
    {
        return FakeDatabase.Instance.Products.Take(4).ToList();
    }
}
