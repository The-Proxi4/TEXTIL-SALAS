using System.Collections.Generic;

namespace textil_salas.Models
{
    public static class FakeDatabase
    {
        public static List<User> Users { get; set; } = new();
        
        // 👇 aquí guardamos temporalmente quién está reseteando
        public static string? ResetEmail { get; set; }
    }
}