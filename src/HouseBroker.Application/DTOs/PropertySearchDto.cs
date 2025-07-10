using System.ComponentModel.DataAnnotations;

namespace HouseBroker.Application.DTOs;

public class PropertySearchDto
{
    // Location filters
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }

    // Price filters
    [Range(0, double.MaxValue)]
    public decimal? MinPrice { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? MaxPrice { get; set; }

    // Property details
    [Range(0, 20)]
    public int? MinBedrooms { get; set; }

    [Range(0, 20)]
    public int? MaxBedrooms { get; set; }

    [Range(0, 20)]
    public int? MinBathrooms { get; set; }

    [Range(0, 20)]
    public int? MaxBathrooms { get; set; }

    [Range(0, int.MaxValue)]
    public int? MinSquareFeet { get; set; }

    [Range(0, int.MaxValue)]
    public int? MaxSquareFeet { get; set; }

    // Property type and status
    public string? PropertyType { get; set; }
    public string? Status { get; set; }

    // Year built
    [Range(1800, 2100)]
    public int? MinYearBuilt { get; set; }

    [Range(1800, 2100)]
    public int? MaxYearBuilt { get; set; }

    // Features
    public string? Features { get; set; }

    // Sorting
    public string? SortBy { get; set; } // "price", "date", "bedrooms", "squarefeet"
    public bool SortDescending { get; set; } = true;

    // Pagination
    [Range(1, 100)]
    public int Page { get; set; } = 1;

    [Range(1, 50)]
    public int PageSize { get; set; } = 10;
}