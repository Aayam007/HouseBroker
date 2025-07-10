using System.ComponentModel.DataAnnotations;

namespace HouseBroker.Domain.Entities;

public class Property : BaseEntity
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
    public decimal Price { get; set; }

    [Required]
    public int Bedrooms { get; set; }

    [Required]
    public int Bathrooms { get; set; }

    [Required]
    public int SquareFeet { get; set; }

    [Required]
    [MaxLength(50)]
    public string PropertyType { get; set; } = string.Empty; // House, Apartment, Condo, etc.

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty; // For Sale, For Rent, Sold, etc.

    public int? YearBuilt { get; set; }

    [MaxLength(500)]
    public string? Features { get; set; } // Comma-separated features

    [MaxLength(255)]
    public string? MainImageUrl { get; set; }

    public bool IsFeatured { get; set; } = false;

    // Foreign key
    public string BrokerId { get; set; } = string.Empty;

    // Commission information
    public decimal? CommissionRate { get; set; }
    public decimal? CommissionAmount { get; set; }

}