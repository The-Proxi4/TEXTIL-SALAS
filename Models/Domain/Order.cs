using System.ComponentModel.DataAnnotations;

namespace textil_salas.Models;

public class Order
{
    [Key]
    public string Id { get; set; } = string.Empty;

    [Required]
    public string OrderNumber { get; set; } = string.Empty;

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string ShippingAddress { get; set; } = string.Empty;

    [Required]
    public string District { get; set; } = string.Empty;

    [Required]
    public string Phone { get; set; } = string.Empty;

    public string? Reference { get; set; }

    [Required]
    public string PaymentMethod { get; set; } = string.Empty;

    [Required]
    public decimal Total { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<OrderItem> Items { get; set; } = new();
}
