namespace textil_salas.Models;

public class CartViewModel
{
    public List<CartItem> Items { get; set; } = new();

    public decimal Total => Items.Sum(item => item.Quantity * item.UnitPrice);
}
