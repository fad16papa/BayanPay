using BayanPay.Common.Models;

namespace BayanPay.UserService.Domain
{
    public class AppUser : UserModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ClerkUserId { get; set; }  // from Clerk
        public string Role { get; set; }  // Optional: agent, admin, etc.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}