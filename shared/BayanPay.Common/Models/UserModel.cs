using System.ComponentModel.DataAnnotations;

namespace BayanPay.Common.Models
{
    public class UserModel
    {
        [Required]
        public Guid UserID { get; set; } = Guid.NewGuid();
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public DateOnly BirthDate { get; set; }
        [Required]
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdateDateTime { get; set; } = DateTime.UtcNow;
    }
}