using FluentAssertions;
using HouseBroker.Domain.Entities;
using HouseBroker.Infrastructure.Data;
using HouseBroker.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HouseBroker.Tests.Services;

public class CommissionServiceTests
{
    private readonly DbContextOptions<HouseBrokerDbContext> _options;

    public CommissionServiceTests()
    {
        _options = new DbContextOptionsBuilder<HouseBrokerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task CalculateCommissionAsync_WithValidPrice_ReturnsCorrectCommission()
    {
        // Arrange
        using var context = new HouseBrokerDbContext(_options);
        var service = new CommissionService(context);
        var propertyPrice = 1000000m; // 10 lakhs

        // Act
        var commission = await service.CalculateCommissionAsync(propertyPrice);

        // Assert
        commission.Should().Be(20000m); // 2% of 10 lakhs = 20,000
    }

    [Theory]
    [InlineData(3000000, 2.0)] // 30 lakhs - should return 2%
    [InlineData(7500000, 1.75)] // 75 lakhs - should return 1.75%
    [InlineData(15000000, 1.5)] // 1.5 crore - should return 1.5%
    public async Task GetCommissionRateAsync_WithDifferentPrices_ReturnsCorrectDefaultRates(decimal price, decimal expectedRate)
    {
        // Arrange
        using var context = new HouseBrokerDbContext(_options);
        var service = new CommissionService(context);

        // Act
        var rate = await service.GetCommissionRateAsync(price);

        // Assert
        rate.Should().Be(expectedRate);
    }

    [Fact]
    public async Task GetCommissionRateAsync_WithDatabaseRate_ReturnsDatabaseRate()
    {
        // Arrange
        using var context = new HouseBrokerDbContext(_options);
        var service = new CommissionService(context);
        
        var commissionRate = new CommissionRate
        {
            Id = Guid.NewGuid(),
            MinPrice = 1000000m,
            MaxPrice = 5000000m,
            RatePercentage = 2.5m,
            Description = "Test Rate",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        
        context.CommissionRates.Add(commissionRate);
        await context.SaveChangesAsync();

        // Act
        var rate = await service.GetCommissionRateAsync(2000000m); // 20 lakhs

        // Assert
        rate.Should().Be(2.5m);
    }

    [Fact]
    public async Task GetCommissionRateAsync_WithInactiveRate_ReturnsDefaultRate()
    {
        // Arrange
        using var context = new HouseBrokerDbContext(_options);
        var service = new CommissionService(context);
        
        var commissionRate = new CommissionRate
        {
            Id = Guid.NewGuid(),
            MinPrice = 1000000m,
            MaxPrice = 5000000m,
            RatePercentage = 2.5m,
            Description = "Inactive Rate",
            IsActive = false,
            CreatedAt = DateTime.UtcNow
        };
        
        context.CommissionRates.Add(commissionRate);
        await context.SaveChangesAsync();

        // Act
        var rate = await service.GetCommissionRateAsync(2000000m); // 20 lakhs

        // Assert
        rate.Should().Be(2.0m); // Should return default rate, not database rate
    }

    [Fact]
    public async Task GetCommissionRateAsync_WithPriceOutsideRange_ReturnsDefaultRate()
    {
        // Arrange
        using var context = new HouseBrokerDbContext(_options);
        var service = new CommissionService(context);
        
        var commissionRate = new CommissionRate
        {
            Id = Guid.NewGuid(),
            MinPrice = 1000000m,
            MaxPrice = 5000000m,
            RatePercentage = 2.5m,
            Description = "Test Rate",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        
        context.CommissionRates.Add(commissionRate);
        await context.SaveChangesAsync();

        // Act
        var rate = await service.GetCommissionRateAsync(6000000m); // 60 lakhs (outside range)

        // Assert
        rate.Should().Be(1.75m); // Should return default rate for this price range
    }

    [Fact]
    public async Task GetActiveCommissionRatesAsync_ReturnsOnlyActiveRates()
    {
        // Arrange
        using var context = new HouseBrokerDbContext(_options);
        var service = new CommissionService(context);
        
        var activeRate = new CommissionRate
        {
            Id = Guid.NewGuid(),
            MinPrice = 1000000m,
            MaxPrice = 5000000m,
            RatePercentage = 2.5m,
            Description = "Active Rate",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        
        var inactiveRate = new CommissionRate
        {
            Id = Guid.NewGuid(),
            MinPrice = 5000000m,
            MaxPrice = 10000000m,
            RatePercentage = 2.0m,
            Description = "Inactive Rate",
            IsActive = false,
            CreatedAt = DateTime.UtcNow
        };
        
        context.CommissionRates.AddRange(activeRate, inactiveRate);
        await context.SaveChangesAsync();

        // Act
        var rates = await service.GetActiveCommissionRatesAsync();

        // Assert
        rates.Should().HaveCount(1);
        rates.First().IsActive.Should().BeTrue();
        rates.First().RatePercentage.Should().Be(2.5m);
    }

    [Fact]
    public async Task GetActiveCommissionRatesAsync_ReturnsRatesOrderedByMinPrice()
    {
        // Arrange
        using var context = new HouseBrokerDbContext(_options);
        var service = new CommissionService(context);
        
        var rate1 = new CommissionRate
        {
            Id = Guid.NewGuid(),
            MinPrice = 5000000m,
            MaxPrice = 10000000m,
            RatePercentage = 2.0m,
            Description = "Higher Min Price",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        
        var rate2 = new CommissionRate
        {
            Id = Guid.NewGuid(),
            MinPrice = 1000000m,
            MaxPrice = 5000000m,
            RatePercentage = 2.5m,
            Description = "Lower Min Price",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        
        context.CommissionRates.AddRange(rate1, rate2);
        await context.SaveChangesAsync();

        // Act
        var rates = await service.GetActiveCommissionRatesAsync();

        // Assert
        rates.Should().HaveCount(2);
        rates.First().MinPrice.Should().Be(1000000m);
        rates.Last().MinPrice.Should().Be(5000000m);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(100000, 2000)] // 2% of 1 lakh
    [InlineData(5000000, 87500)] // 1.75% of 50 lakhs
    [InlineData(20000000, 300000)] // 1.5% of 2 crore
    public async Task CalculateCommissionAsync_WithVariousPrices_CalculatesCorrectly(decimal price, decimal expectedCommission)
    {
        // Arrange
        using var context = new HouseBrokerDbContext(_options);
        var service = new CommissionService(context);

        // Act
        var commission = await service.CalculateCommissionAsync(price);

        // Assert
        commission.Should().Be(expectedCommission);
    }
} 