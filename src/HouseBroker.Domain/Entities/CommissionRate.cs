using System.ComponentModel.DataAnnotations;

namespace HouseBroker.Domain.Entities;

public class CommissionRate : BaseEntity
{
    [Required]
    public decimal MinPrice { get; set; }

    public decimal? MaxPrice { get; set; } // null means no upper limit

    [Required]
    [Range(0, 100)]
    public decimal RatePercentage { get; set; }

    [Required]
    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}