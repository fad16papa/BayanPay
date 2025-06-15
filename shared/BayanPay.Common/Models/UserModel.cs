using System.ComponentModel.DataAnnotations;

namespace BayanPay.Common.Models
{
    public class UserModel : AuditModel
    {
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
    }
}