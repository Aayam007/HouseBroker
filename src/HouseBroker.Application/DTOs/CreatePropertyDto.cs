using System.ComponentModel.DataAnnotations;

namespace HouseBroker.Application.DTOs;

public class CreatePropertyDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string City { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string State { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
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
    [MaxLength(50)]
    public string PropertyType { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    [Range(1800, 2100)]
    public int? YearBuilt { get; set; }

    [MaxLength(500)]
    public string? Features { get; set; }

    [MaxLength(255)]
    public string? MainImageUrl { get; set; }

    public bool IsFeatured { get; set; } = false;

    public List<string> ImageUrls { get; set; } = new List<string>();
}