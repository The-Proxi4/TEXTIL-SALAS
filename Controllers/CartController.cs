using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using textil_salas.Models;

namespace textil_salas.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public CartController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var model = new CartViewModel
        {
            Items = _dbContext.CartItems
                .Where(ci => ci.UserId == userId)
                .ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddToCart(string productId, int quantity = 1)
    {
        if (string.IsNullOrWhiteSpace(productId) || quantity < 1)
        {
            TempData["Error"] = "Selecciona un producto válido y una cantidad mayor o igual a 1.";
            return RedirectToAction("Shop", "Home");
        }

        var product = FakeDatabase.Instance.Products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            TempData["Error"] = "El producto no se encontró.";
            return RedirectToAction("Shop", "Home");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var cartItem = _dbContext.CartItems.FirstOrDefault(ci => ci.UserId == userId && ci.ProductId == productId);
        if (cartItem == null)
        {
            cartItem = new CartItem
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                ProductId = product.Id,
                ProductName = product.Name,
                ImageUrl = product.ImageUrl,
                UnitPrice = product.Price,
                Quantity = quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _dbContext.CartItems.Add(cartItem);
        }
        else
        {
            cartItem.Quantity += quantity;
            cartItem.UpdatedAt = DateTime.UtcNow;
            _dbContext.CartItems.Update(cartItem);
        }

        _dbContext.SaveChanges();
        TempData["Success"] = "Producto agregado al carrito correctamente.";
        return RedirectToAction("Shop", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateQuantity(string id, int quantity)
    {
        if (string.IsNullOrWhiteSpace(id) || quantity < 1)
        {
            TempData["Error"] = "La cantidad debe ser al menos 1.";
            return RedirectToAction(nameof(Index));
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cartItem = _dbContext.CartItems.FirstOrDefault(ci => ci.Id == id && ci.UserId == userId);
        if (cartItem == null)
        {
            TempData["Error"] = "Artículo de carrito no encontrado.";
            return RedirectToAction(nameof(Index));
        }

        cartItem.Quantity = quantity;
        cartItem.UpdatedAt = DateTime.UtcNow;
        _dbContext.SaveChanges();

        TempData["Success"] = "Cantidad actualizada.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cartItem = _dbContext.CartItems.FirstOrDefault(ci => ci.Id == id && ci.UserId == userId);
        if (cartItem == null)
        {
            TempData["Error"] = "Artículo de carrito no encontrado.";
            return RedirectToAction(nameof(Index));
        }

        _dbContext.CartItems.Remove(cartItem);
        _dbContext.SaveChanges();

        TempData["Success"] = "Producto eliminado del carrito.";
        return RedirectToAction(nameof(Index));
    }
}
