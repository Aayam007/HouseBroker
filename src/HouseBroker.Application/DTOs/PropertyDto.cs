namespace HouseBroker.Application.DTOs;

public class PropertyDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public int SquareFeet { get; set; }
    public string PropertyType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int? YearBuilt { get; set; }
    public string? Features { get; set; }
    public string? MainImageUrl { get; set; }
    public bool IsFeatured { get; set; }
    public Guid BrokerId { get; set; }
    public string BrokerName { get; set; } = string.Empty;
    public string BrokerEmail { get; set; } = string.Empty;
    public string BrokerPhone { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }


    // Commission information (only visible to broker)
    public decimal? CommissionRate { get; set; }
    public decimal? CommissionAmount { get; set; }
    public bool IsOwner { get; set; } = false;
}