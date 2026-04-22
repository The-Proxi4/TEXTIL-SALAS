using System.ComponentModel.DataAnnotations;

namespace textil_salas.Models;

public class Product
{
    [Required(ErrorMessage = "El ID es obligatorio")]
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "La categoría es obligatoria")]
    public string Category { get; set; } = string.Empty;

    public string? Subcategory { get; set; }

    [Url(ErrorMessage = "La URL de la imagen no es válida")]
    public string ImageUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "El material es obligatorio")]
    public string Material { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "El GSM debe ser mayor a 0")]
    public int Gsm { get; set; }

    public string[] Sizes { get; set; } = [];

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad mínima B2B debe ser mayor a 0")]
    public int? B2BMinQty { get; set; }

    public bool IsActive { get; set; } = true;
}
