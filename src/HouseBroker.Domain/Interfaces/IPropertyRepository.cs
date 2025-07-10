using HouseBroker.Domain.Entities;

namespace HouseBroker.Domain.Interfaces;

public interface IPropertyRepository : IRepository<Property>
{
    Task<IEnumerable<Property>> GetPropertiesByBrokerAsync(string brokerId);
    Task<IEnumerable<Property>> SearchPropertiesAsync(string? city, string? state, string? zipCode, decimal? minPrice, decimal? maxPrice,
        int? minBedrooms, int? maxBedrooms, int? minBathrooms, int? maxBathrooms, int? minSquareFeet, int? maxSquareFeet,
        string? propertyType, string? status, int? minYearBuilt, int? maxYearBuilt, string? features, string? sortBy,
        bool sortDescending, int page, int pageSize);
    Task<IEnumerable<Property>> GetFeaturedPropertiesAsync();
    Task<IEnumerable<Property>> GetPropertiesByStatusAsync(string status);
}