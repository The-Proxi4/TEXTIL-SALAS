using System.ComponentModel.DataAnnotations;
using textil_salas.Models;

namespace textil_salas.ViewModels;

public class ClientViewModel
{
    public string Id { get; set; } = string.Empty;

    [Required]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? DeactivatedAt { get; set; }

    public static ClientViewModel FromUser(ApplicationUser u) => new()
    {
        Id = u.Id,
        NombreCompleto = u.NombreCompleto,
        Email = u.Email ?? string.Empty,
        PhoneNumber = u.PhoneNumber,
        IsActive = u.IsActive,
        DeactivatedAt = u.DeactivatedAt
    };
}
