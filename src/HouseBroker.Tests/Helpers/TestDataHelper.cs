using HouseBroker.Domain.Entities;
using HouseBroker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Tests.Helpers;

public static class TestDataHelper
{
    public static Property CreateSampleProperty(
        Guid? id = null,
        string title = "Test Property",
        decimal price = 1000000m,
        string city = "Test City",
        bool isDeleted = false,
        bool isFeatured = false,
        string brokerId = "test-broker-id")
    {
        return new Property
        {
            Id = id ?? Guid.NewGuid(),
            Title = title,
            Description = "Test Description",
            Address = "123 Test St",
            City = city,
            State = "Test State",
            ZipCode = "12345",
            Price = price,
            PropertyType = "House",
            Status = "Available",
            Bedrooms = 3,
            Bathrooms = 2,
            SquareFeet = 1500,
            YearBuilt = 2020,
            BrokerId = brokerId,
            IsDeleted = isDeleted,
            IsFeatured = isFeatured,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static CommissionRate CreateSampleCommissionRate(
        Guid? id = null,
        decimal minPrice = 1000000m,
        decimal? maxPrice = 5000000m,
        decimal ratePercentage = 2.5m,
        bool isActive = true)
    {
        return new CommissionRate
        {
            Id = id ?? Guid.NewGuid(),
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            RatePercentage = ratePercentage,
            Description = "Test Commission Rate",
            IsActive = isActive,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static async Task SeedTestData(HouseBrokerDbContext context)
    {
        // Add sample properties
        var properties = new List<Property>
        {
            CreateSampleProperty(title: "Mumbai Property 1", city: "Mumbai", price: 2000000m),
            CreateSampleProperty(title: "Mumbai Property 2", city: "Mumbai", price: 3000000m),
            CreateSampleProperty(title: "Delhi Property", city: "Delhi", price: 2500000m),
            CreateSampleProperty(title: "Featured Property", isFeatured: true, price: 1500000m),
            CreateSampleProperty(title: "Deleted Property", isDeleted: true, price: 1000000m)
        };

        context.Properties.AddRange(properties);

        // Add sample commission rates
        var commissionRates = new List<CommissionRate>
        {
            CreateSampleCommissionRate(minPrice: 1000000m, maxPrice: 5000000m, ratePercentage: 2.5m),
            CreateSampleCommissionRate(minPrice: 5000000m, maxPrice: 10000000m, ratePercentage: 2.0m),
            CreateSampleCommissionRate(minPrice: 10000000m, maxPrice: null, ratePercentage: 1.5m),
            CreateSampleCommissionRate(minPrice: 2000000m, maxPrice: 4000000m, ratePercentage: 2.2m, isActive: false)
        };

        context.CommissionRates.AddRange(commissionRates);

        await context.SaveChangesAsync();
    }

    public static HouseBrokerDbContext CreateInMemoryDbContext(string? databaseName = null)
    {
        var options = new DbContextOptionsBuilder<HouseBrokerDbContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
            .Options;

        return new HouseBrokerDbContext(options);
    }
} 