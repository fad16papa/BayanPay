using System.ComponentModel.DataAnnotations;

public class AuditModel
{
    [Required]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    [Required]
    public DateTime UpdateDateTime { get; set; }
    [Required]
    public string CreatedBy { get; set; }
}