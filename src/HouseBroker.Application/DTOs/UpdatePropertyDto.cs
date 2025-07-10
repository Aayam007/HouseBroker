using System.ComponentModel.DataAnnotations;

namespace HouseBroker.Application.DTOs;

public class UpdatePropertyDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string State { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string ZipCode { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    [Range(0, 20)]
    public int Bedrooms { get; set; }

    [Required]
    [Range(0, 20)]
    public int Bathrooms { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int SquareFeet { get; set; }

    [Required]
    [StringLength(50)]
    public string PropertyType { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = string.Empty;

    [Range(1800, 2100)]
    public int? YearBuilt { get; set; }

    [StringLength(500)]
    public string? Features { get; set; }

    [StringLength(255)]
    public string? MainImageUrl { get; set; }

    public bool IsFeatured { get; set; }
} 