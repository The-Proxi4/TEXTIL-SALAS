using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using textil_salas.Models;

namespace textil_salas.Controllers;

[Authorize]
public class CheckoutController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public CheckoutController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var items = _dbContext.CartItems.Where(ci => ci.UserId == userId).ToList();
        if (!items.Any())
        {
            TempData["Error"] = "No tienes productos en el carrito para realizar el checkout.";
            return RedirectToAction("Index", "Cart");
        }

        var model = new CheckoutViewModel
        {
            Items = items
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PlaceOrder(CheckoutViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return RedirectToAction("Login", "Account");

        model.Items = _dbContext.CartItems.Where(ci => ci.UserId == userId).ToList();
        if (!model.Items.Any())
        {
            TempData["Error"] = "No tienes productos en el carrito.";
            return RedirectToAction("Index", "Cart");
        }

        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }

        var order = new Order
        {
            Id = Guid.NewGuid().ToString(),
            OrderNumber = GenerateOrderNumber(),
            UserId = userId,
            ShippingAddress = model.ShippingAddress,
            District = model.District,
            Phone = model.Phone,
            Reference = model.Reference,
            PaymentMethod = model.PaymentMethod,
            Total = model.Total,
            CreatedAt = DateTime.UtcNow,
        };

        foreach (var item in model.Items)
        {
            order.Items.Add(new OrderItem
            {
                Id = Guid.NewGuid().ToString(),
                OrderId = order.Id,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ImageUrl = item.ImageUrl,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity,
                Subtotal = item.UnitPrice * item.Quantity
            });
        }

        _dbContext.Orders.Add(order);
        _dbContext.CartItems.RemoveRange(model.Items);
        _dbContext.SaveChanges();

        return RedirectToAction("Confirmed", new { orderId = order.OrderNumber });
    }

    [HttpGet]
    public IActionResult Confirmed(string orderId)
    {
        if (string.IsNullOrWhiteSpace(orderId))
            return RedirectToAction("Index", "Home");

        ViewData["OrderNumber"] = orderId;
        return View();
    }

    private static string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}";
    }
}
