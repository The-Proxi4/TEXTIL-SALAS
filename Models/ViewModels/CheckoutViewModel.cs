using System.ComponentModel.DataAnnotations;

namespace textil_salas.Models;

public class CheckoutViewModel
{
    public List<CartItem> Items { get; set; } = new();

    public decimal Total => Items.Sum(i => i.UnitPrice * i.Quantity);

    [Required(ErrorMessage = "La dirección de envío es obligatoria")]
    [Display(Name = "Dirección")]
    public string ShippingAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "El distrito es obligatorio")]
    [Display(Name = "Distrito")]
    public string District { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [Display(Name = "Teléfono")]
    public string Phone { get; set; } = string.Empty;

    [Display(Name = "Referencia")]
    public string Reference { get; set; } = string.Empty;

    [Required(ErrorMessage = "Selecciona un método de pago")]
    [Display(Name = "Método de pago")]
    public string PaymentMethod { get; set; } = string.Empty;
}
