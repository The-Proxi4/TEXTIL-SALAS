using System.Collections.Generic;

namespace textil_salas.Models
{
    public interface IFakeDatabase
    {
        List<User> Users { get; set; }
        string? ResetEmail { get; set; }
        List<Categoria> Categorias { get; set; }
        List<Product> Products { get; set; }
        int NextCategoriaId { get; set; }
        int NextProductId { get; set; }
    }
}