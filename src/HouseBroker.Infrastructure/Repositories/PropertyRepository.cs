using HouseBroker.Domain.Entities;
using HouseBroker.Domain.Interfaces;
using HouseBroker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly HouseBrokerDbContext _context;

    public PropertyRepository(HouseBrokerDbContext context)
    {
        _context = context;
    }

    public async Task<Property?> GetByIdAsync(Guid id)
    {
        return await _context.Properties
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
    }

    public async Task<IEnumerable<Property>> GetAllAsync()
    {
        return await _context.Properties
            .Where(p => !p.IsDeleted)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> FindAsync(Func<Property, bool> predicate)
    {
        var properties = await _context.Properties
            .Where(p => !p.IsDeleted)
            .ToListAsync();

        return properties.Where(predicate);
    }

    public async Task<Property> AddAsync(Property entity)
    {
        _context.Properties.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Property entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Properties.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Property entity)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Properties.AnyAsync(p => p.Id == id && !p.IsDeleted);
    }

    public async Task<IEnumerable<Property>> GetPropertiesByBrokerAsync(string brokerId)
    {
        return await _context.Properties
            .Where(p => p.BrokerId == brokerId && !p.IsDeleted)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> SearchPropertiesAsync(string? city, string? state, string? zipCode, decimal? minPrice, decimal? maxPrice,
        int? minBedrooms, int? maxBedrooms, int? minBathrooms, int? maxBathrooms, int? minSquareFeet, int? maxSquareFeet,
        string? propertyType, string? status, int? minYearBuilt, int? maxYearBuilt, string? features, string? sortBy,
        bool sortDescending, int page, int pageSize)
    {
        var query = _context.Properties
            .Where(p => !p.IsDeleted);

        // Location filters
        if (!string.IsNullOrEmpty(city))
            query = query.Where(p => p.City.Contains(city, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(state))
            query = query.Where(p => p.State.Contains(state, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(zipCode))
            query = query.Where(p => p.ZipCode.Contains(zipCode, StringComparison.OrdinalIgnoreCase));

        // Price filters
        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        // Bedroom filters
        if (minBedrooms.HasValue)
            query = query.Where(p => p.Bedrooms >= minBedrooms.Value);

        if (maxBedrooms.HasValue)
            query = query.Where(p => p.Bedrooms <= maxBedrooms.Value);

        // Bathroom filters
        if (minBathrooms.HasValue)
            query = query.Where(p => p.Bathrooms >= minBathrooms.Value);

        if (maxBathrooms.HasValue)
            query = query.Where(p => p.Bathrooms <= maxBathrooms.Value);

        // Square feet filters
        if (minSquareFeet.HasValue)
            query = query.Where(p => p.SquareFeet >= minSquareFeet.Value);

        if (maxSquareFeet.HasValue)
            query = query.Where(p => p.SquareFeet <= maxSquareFeet.Value);

        // Property type and status
        if (!string.IsNullOrEmpty(propertyType))
            query = query.Where(p => p.PropertyType == propertyType);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(p => p.Status == status);

        // Year built filters
        if (minYearBuilt.HasValue)
            query = query.Where(p => p.YearBuilt >= minYearBuilt.Value);

        if (maxYearBuilt.HasValue)
            query = query.Where(p => p.YearBuilt <= maxYearBuilt.Value);

        // Features filter
        if (!string.IsNullOrEmpty(features))
            query = query.Where(p => p.Features != null && p.Features.Contains(features, StringComparison.OrdinalIgnoreCase));

        // Sorting
        query = sortBy?.ToLower() switch
        {
            "price" => sortDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
            "bedrooms" => sortDescending ? query.OrderByDescending(p => p.Bedrooms) : query.OrderBy(p => p.Bedrooms),
            "squarefeet" => sortDescending ? query.OrderByDescending(p => p.SquareFeet) : query.OrderBy(p => p.SquareFeet),
            _ => sortDescending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt)
        };

        // Pagination
        var skip = (page - 1) * pageSize;
        query = query.Skip(skip).Take(pageSize);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Property>> GetFeaturedPropertiesAsync()
    {
        return await _context.Properties
            .Where(p => p.IsFeatured && !p.IsDeleted)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> GetPropertiesByStatusAsync(string status)
    {
        return await _context.Properties
            .Where(p => p.Status == status && !p.IsDeleted)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }
}