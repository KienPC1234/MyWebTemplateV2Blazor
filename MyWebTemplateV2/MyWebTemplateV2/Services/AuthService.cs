using Microsoft.EntityFrameworkCore;
using MyWebTemplateV2.Data;
using MyWebTemplateV2.Client.Models;
using System.Security.Cryptography;
using System.Text;

namespace MyWebTemplateV2.Services
{
    public class AuthService
    {
        private readonly SystemDbContext _context;
        public bool IsLoggedIn { get; private set; }

        public AuthService(SystemDbContext context)
        {
            _context = context;
        }

        public async Task<bool> VerifyAdminAsync(string username, string password)
        {
            var user = await _context.AdminUsers.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return false;

            // Simple hash verification
            var hash = ComputeHash(password);
            var success = user.PasswordHash == hash;
            if (success) IsLoggedIn = true;
            return success;
        }

        public void Logout() => IsLoggedIn = false;

        public string ComputeHash(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        public async Task CreateInitialAdminAsync(string username, string password)
        {
            if (!await _context.AdminUsers.AnyAsync())
            {
                var user = new AdminUser
                {
                    Username = username,
                    PasswordHash = ComputeHash(password)
                };
                _context.AdminUsers.Add(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
