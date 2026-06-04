using Microsoft.AspNetCore.Identity;

namespace textil_salas.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime? DeactivatedAt { get; set; }
    }
}