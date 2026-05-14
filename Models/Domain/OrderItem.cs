using System.ComponentModel.DataAnnotations;

namespace textil_salas.Models;

public class OrderItem
{
    [Key]
    public string Id { get; set; } = string.Empty;

    [Required]
    public string OrderId { get; set; } = string.Empty;

    [Required]
    public string ProductId { get; set; } = string.Empty;

    [Required]
    public string ProductName { get; set; } = string.Empty;

    public string? ImageUrl { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal Subtotal { get; set; }
}
