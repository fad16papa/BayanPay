using System.ComponentModel.DataAnnotations;
using BayanPay.Common.Models;

namespace BayanPay.UserService.Domain
{
    public class AppUser : UserModel
    {
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string ClerkUserId { get; set; }  // from Clerk
        [Required]
        public string Role { get; set; }  // Optional: agent, admin, etc.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}