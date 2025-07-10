using HouseBroker.Domain.Entities;
using HouseBroker.Domain.Interfaces;
using HouseBroker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Services;

public class CommissionService : ICommissionService
{
    private readonly HouseBrokerDbContext _context;

    public CommissionService(HouseBrokerDbContext context)
    {
        _context = context;
    }

    public async Task<decimal> CalculateCommissionAsync(decimal propertyPrice)
    {
        var rate = await GetCommissionRateAsync(propertyPrice);
        return propertyPrice * (rate / 100);
    }

    public async Task<decimal> GetCommissionRateAsync(decimal propertyPrice)
    {
        // First try to get from database
        var dbRate = await _context.CommissionRates
            .Where(cr => cr.IsActive &&
                        cr.MinPrice <= propertyPrice &&
                        (cr.MaxPrice == null || cr.MaxPrice > propertyPrice))
            .OrderByDescending(cr => cr.MinPrice)
            .FirstOrDefaultAsync();

        if (dbRate != null)
            return dbRate.RatePercentage;

        // Fallback to default logic if no database rates configured
        return GetDefaultCommissionRate(propertyPrice);
    }

    public async Task<IEnumerable<CommissionRate>> GetActiveCommissionRatesAsync()
    {
        return await _context.CommissionRates
            .Where(cr => cr.IsActive)
            .OrderBy(cr => cr.MinPrice)
            .ToListAsync();
    }

    private static decimal GetDefaultCommissionRate(decimal propertyPrice)
    {
        // Default commission logic as specified
        if (propertyPrice < 5000000) // < 50,00,000
            return 2.0m;
        else if (propertyPrice <= 10000000) // 50,00,000 to 1 crore
            return 1.75m;
        else // > 1 crore
            return 1.5m;
    }
}