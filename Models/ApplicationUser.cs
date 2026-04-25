using Microsoft.AspNetCore.Identity;

namespace textil_salas.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; } = string.Empty;
    }
}