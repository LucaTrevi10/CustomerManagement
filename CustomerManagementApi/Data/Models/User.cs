using Microsoft.AspNetCore.Identity;

namespace CustomerManagementApi.Data.Models
{
    public class User :IdentityUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Hashed password
        public string Role { get; set; } = "User"; // Ruoli, ad esempio "Admin", "User"
    }
}
