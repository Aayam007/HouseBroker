namespace HouseBroker.Application.DTOs;

public class CommissionRateDto
{
    public Guid Id { get; set; }
    public string PropertyType { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
} 