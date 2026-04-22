using System.Collections.Generic;

namespace textil_salas.Models
{
    public static class FakeDatabase
    {
        public static List<User> Users { get; set; } = new();

        // Para recuperación de contraseña
        public static string? ResetEmail { get; set; }

        // ✅ Categorías
        public static List<Categoria> Categorias { get; set; } = new()
        {
            new Categoria { Id = 1, Nombre = "Toallas de baño",  Descripcion = "Toallas grandes", Activo = true },
            new Categoria { Id = 2, Nombre = "Toallas de mano",  Descripcion = "Toallas pequeñas", Activo = true },
            new Categoria { Id = 3, Nombre = "Toallas de piso",  Descripcion = "Alfombras", Activo = true },
            new Categoria { Id = 4, Nombre = "Batas",            Descripcion = "Batas de baño", Activo = true },
            new Categoria { Id = 5, Nombre = "Descontinuados",   Descripcion = "Sin stock", Activo = false },
        };

        // ✅ Productos
public static List<Product> Products { get; set; } = new()
{
    new Product
    {
        Id = "1",
        Name = "Toalla de Mano Blanca",
        Description = "Algodón prima 700 GSM, ideal para baños de hotel y uso diario.",
        Price = 70,
        Category = "Toallas de mano",
        Subcategory = "Mano",
        ImageUrl = "/images/towel-hand.jpg",
        Sizes = new [] { "40x70cm", "50x80cm" }
    },
    new Product
    {
        Id = "2",
        Name = "Toalla de Piso Blanca",
        Description = "Alfombra de baño antideslizante de algodón prima 750 GSM.",
        Price = 85,
        Category = "Toallas de piso",
        Subcategory = "Piso",
        ImageUrl = "/images/towel-floor.jpg",
        Sizes = new [] { "50x70cm", "60x90cm" }
    },
    new Product
    {
        Id = "3",
        Name = "Toalla de Baño Blanca",
        Description = "Toalla de cuerpo completo en algodón prima 750 GSM.",
        Price = 95,
        Category = "Toallas de baño",
        Subcategory = "Baño",
        ImageUrl = "/images/towel-white.jpg",
        Sizes = new [] { "70x140cm", "80x160cm" }
    },
    new Product
    {
        Id = "4",
        Name = "Bata de Baño Blanca",
        Description = "Bata terry cloth premium de algodón prima 700 GSM.",
        Price = 100,
        Category = "Batas",
        Subcategory = "Bata",
        ImageUrl = "/images/robe-white.jpg",
        Sizes = new [] { "S", "M", "L", "XL" }
    }
};

        // Auto incremento
        public static int NextCategoriaId { get; set; } = 6;
    }
}