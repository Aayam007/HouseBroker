using HouseBroker.Domain.Entities;

namespace HouseBroker.Domain.Interfaces;

public interface ICommissionService
{
    Task<decimal> CalculateCommissionAsync(decimal propertyPrice);
    Task<decimal> GetCommissionRateAsync(decimal propertyPrice);
    Task<IEnumerable<CommissionRate>> GetActiveCommissionRatesAsync();
}