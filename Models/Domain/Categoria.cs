using System.ComponentModel.DataAnnotations;

namespace textil_salas.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        // CA-4: Soft delete — nunca se borra físicamente
        public bool Activo { get; set; } = true;
    }
}