using System.ComponentModel.DataAnnotations;

namespace textil_salas.Models;

public class B2BQuoteViewModel
{
    [Required]
    [Display(Name = "Empresa")]
    public string Company { get; set; } = string.Empty;

    [Required]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "El RUC debe tener 11 dígitos")]
    public string Ruc { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Contacto")]
    public string ContactName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Teléfono")]
    public string? Phone { get; set; }

    [Required]
    [Display(Name = "Detalle del pedido")]
    public string Message { get; set; } = string.Empty;
}
