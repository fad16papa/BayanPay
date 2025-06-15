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

        public AppUser(Guid id, string firstName, string lastName, string email, string address, DateOnly birthDate, string clerkUserId, string role)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Address = address;
            BirthDate = birthDate;
            ClerkUserId = clerkUserId;
            Role = role;
            CreatedDateTime = DateTime.UtcNow;
            UpdateDateTime = DateTime.UtcNow;
        }
    }
}