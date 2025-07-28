using System.ComponentModel.DataAnnotations;

public class AuditModel
{
    [Required]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    [Required]
    public DateTime UpdateDateTime { get; set; } = DateTime.MinValue;
    [Required]
    public string CreatedBy { get; set; }
}