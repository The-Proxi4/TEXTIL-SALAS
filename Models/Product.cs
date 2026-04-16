namespace textil_salas.Models;

public class Product
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? Subcategory { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Material { get; set; } = string.Empty;
    public int Gsm { get; set; }
    public string[] Sizes { get; set; } = [];
    public int? B2BMinQty { get; set; }
}
