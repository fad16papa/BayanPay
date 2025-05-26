namespace BayanPay.UserService.Domain
{
    public class AppUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ClerkUserId { get; set; }  // from Clerk
        public string Email { get; set; }
        public string Role { get; set; }  // Optional: agent, admin, etc.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}