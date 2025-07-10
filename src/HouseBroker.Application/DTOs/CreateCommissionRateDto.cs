using System.ComponentModel.DataAnnotations;

namespace HouseBroker.Application.DTOs;

public class CreateCommissionRateDto
{
    [Required]
    [StringLength(50)]
    public string PropertyType { get; set; } = string.Empty;

    [Required]
    [Range(0, 100)]
    public decimal Rate { get; set; }

    public bool IsActive { get; set; } = true;
} 