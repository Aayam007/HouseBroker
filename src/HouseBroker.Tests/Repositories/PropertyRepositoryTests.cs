using FluentAssertions;
using HouseBroker.Domain.Entities;
using HouseBroker.Infrastructure.Data;
using HouseBroker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HouseBroker.Tests.Repositories;

public class PropertyRepositoryTests
{
    private readonly DbContextOptions<HouseBrokerDbContext> _options;

    public PropertyRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<HouseBrokerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsProperty()
    {
        // Arrange
        using var context = new HouseBrokerDbContext(_options);
        var repository = new PropertyRepository(context);
        
        var property = new Property
        {
            Id = Guid.NewGuid(),
            Title = "Test Property",
            Description = "Test Description",
            Address = "123 Test St",
            City = "Test City",
            State = "Test State",
            ZipCode = "12345",
            Price = 1000000m,
            PropertyType = "House",
            Status = "Available",
            Bedrooms = 3,
            Bathrooms = 2,
            SquareFeet = 1500,
            YearBuilt = 2020,
            BrokerId = "test-broker-id",
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };
        
        context.Properties.Add(property);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(property.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Test Property");
        result.Price.Should().Be(1000000m);
    }

    [Fact]
    public async Task GetByIdAsync_WithDeletedProperty_ReturnsNull()
    {
        // Arrange
        using var context = new HouseBrokerDbContext(_options);
        var repository = new PropertyRepository(context);
        
        var property = new Property
        {
            Id = Guid.NewGuid(),
            Title = "Test Property",
            Description = "Test Description",
            Address = "123 Test St",
            City = "Test City",
            State = "Test State",
            ZipCode = "12345",
            Price = 1000000m,
            PropertyType = "House",
            Status = "Available",
            IsDeleted = true,
            CreatedAt = DateTime.UtcNow
        };
        
        context.Properties.Add(property);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(property.Id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOnlyNonDeletedProperties()
    {
        // Arrange
        using var context = new HouseBrokerDbContext(_options);
        var repository = new PropertyRepository(context);
        
        var activeProperty = new Property
        {
            Id = Guid.NewGuid(),
            Title = "Active Property",
            Description = "Active Description",
            Address = "123 Active St",
            City = "Active City",
            State = "Active State",
            ZipCode = "12345",
            Price = 1000000m,
            PropertyType = "House",
            Status = "Available",
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };
        
        var deletedProperty = new Property
        {
            Id = Guid.NewGuid(),
            Title = "Deleted Property",
            Description = "Deleted Description",
            Address = "123 Deleted St",
            City = "Deleted City",
            State = "Deleted State",
            ZipCode = "12345",
            Price = 2000000m,
            PropertyType = "House",
            Status = "Available",
            IsDeleted = true,
            CreatedAt = DateTime.UtcNow
        };
        
        context.Properties.AddRange(activeProperty, deletedProperty);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().Title.Should().Be("Active Property");
    }

    [Fact]
    public async Task AddAsync_WithValidProperty_AddsToDatabase()
    {
        // Arrange
        using var context = new HouseBrokerDbContext(_options);
        var repository = new PropertyRepository(context);
        
        var property = new Property
        {
            Id = Guid.NewGuid(),
            Title = "New Property",
            Description = "New Description",
            Address = "123 New St",
            City = "New City",
            State = "New State",
            ZipCode = "12345",
            Price = 1000000m,
            PropertyType = "House",
            Status = "Available",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var result = await repository.AddAsync(property);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(property.Id);
        
        var savedProperty = await context.Properties.FindAsync(property.Id);
        savedProperty.Should().NotBeNull();
        savedProperty!.Title.Should().Be("New Property");
    }

    [Fact]
    public async Task UpdateAsync_WithValidProperty_UpdatesInDatabase()
    {
        // Arrange
        using var context = new HouseBrokerDbContext(_options);
        var repository = new PropertyRepository(context);
        
        var property = new Property
        {
            Id = Guid.NewGuid(),
            Title = "Original Title",
            Description = "Original Description",
            Address = "123 Original St",
            City = "Original City",
            State = "Original State",
            ZipCode = "12345",
            Price = 1000000m,
            PropertyType = "House",
            Status = "Available",
            CreatedAt = DateTime.UtcNow
        };
        
        context.Properties.Add(property);
        await context.SaveChangesAsync();

        // Act
        property.Title = "Updated Title";
        property.Price = 1500000m;
        await repository.UpdateAsync(property);

        // Assert
        var updatedProperty = await context.Properties.FindAsync(property.Id);
        updatedProperty.Should().NotBeNull();
        updatedProperty!.Title.Should().Be("Updated Title");
        updatedProperty.Price.Should().Be(1500000m);
        updatedProperty.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteAsync_WithValidProperty_MarksAsDeleted()
    {
        // Arrange
        using var context = new HouseBrokerDbContext(_options);
        var repository = new PropertyRepository(context);
        
        var property = new Property
        {
            Id = Guid.NewGuid(),
            Title = "To Delete",
            Description = "To Delete Description",
            Address = "123 Delete St",
            City = "Delete City",
            State = "Delete State",
            ZipCode = "12345",
            Price = 1000000m,
            PropertyType = "House",
            Status = "Available",
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };
        
        context.Properties.Add(property);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(property);

        // Assert
        var deletedProperty = await context.Properties.FindAsync(property.Id);
        deletedProperty.Should().NotBeNull();
        deletedProperty!.IsDeleted.Should().BeTrue();
        deletedProperty.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task SearchPropertiesAsync_WithCityFilter_ReturnsFilteredResults()
    {
        // Arrange
        using var context = new HouseBrokerDbContext(_options);
        var repository = new PropertyRepository(context);
        
        var property1 = new Property
        {
            Id = Guid.NewGuid(),
            Title = "Mumbai Property",
            Description = "Mumbai Description",
            Address = "123 Mumbai St",
            City = "Mumbai",
            State = "Maharashtra",
            ZipCode = "400001",
            Price = 1000000m,
            PropertyType = "House",
            Status = "Available",
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };
        
        var property2 = new Property
        {
            Id = Guid.NewGuid(),
            Title = "Delhi Property",
            Description = "Delhi Description",
            Address = "123 Delhi St",
            City = "Delhi",
            State = "Delhi",
            ZipCode = "110001",
            Price = 2000000m,
            PropertyType = "House",
            Status = "Available",
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };
        
        context.Properties.AddRange(property1, property2);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchPropertiesAsync(
            city: "Mumbai",
            state: null,
            zipCode: null,
            minPrice: null,
            maxPrice: null,
            minBedrooms: null,
            maxBedrooms: null,
            minBathrooms: null,
            maxBathrooms: null,
            minSquareFeet: null,
            maxSquareFeet: null,
            propertyType: null,
            status: null,
            minYearBuilt: null,
            maxYearBuilt: null,
            features: null,
            sortBy: null,
            sortDescending: false,
            page: 1,
            pageSize: 10
        );

        // Assert
        result.Should().HaveCount(1);
        result.First().City.Should().Be("Mumbai");
    }

    [Fact]
    public async Task SearchPropertiesAsync_WithPriceFilter_ReturnsFilteredResults()
    {
        // Arrange
        using var context = new HouseBrokerDbContext(_options);
        var repository = new PropertyRepository(context);
        
        var property1 = new Property
        {
            Id = Guid.NewGuid(),
            Title = "Cheap Property",
            Description = "Cheap Description",
            Address = "123 Cheap St",
            City = "Test City",
            State = "Test State",
            ZipCode = "12345",
            Price = 500000m,
            PropertyType = "House",
            Status = "Available",
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };
        
        var property2 = new Property
        {
            Id = Guid.NewGuid(),
            Title = "Expensive Property",
            Description = "Expensive Description",
            Address = "123 Expensive St",
            City = "Test City",
            State = "Test State",
            ZipCode = "12345",
            Price = 2000000m,
            PropertyType = "House",
            Status = "Available",
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };
        
        context.Properties.AddRange(property1, property2);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchPropertiesAsync(
            city: null,
            state: null,
            zipCode: null,
            minPrice: 1000000m,
            maxPrice: null,
            minBedrooms: null,
            maxBedrooms: null,
            minBathrooms: null,
            maxBathrooms: null,
            minSquareFeet: null,
            maxSquareFeet: null,
            propertyType: null,
            status: null,
            minYearBuilt: null,
            maxYearBuilt: null,
            features: null,
            sortBy: null,
            sortDescending: false,
            page: 1,
            pageSize: 10
        );

        // Assert
        result.Should().HaveCount(1);
        result.First().Title.Should().Be("Expensive Property");
    }

    [Fact]
    public async Task GetFeaturedPropertiesAsync_ReturnsOnlyFeaturedProperties()
    {
        // Arrange
        using var context = new HouseBrokerDbContext(_options);
        var repository = new PropertyRepository(context);
        
        var featuredProperty = new Property
        {
            Id = Guid.NewGuid(),
            Title = "Featured Property",
            Description = "Featured Description",
            Address = "123 Featured St",
            City = "Test City",
            State = "Test State",
            ZipCode = "12345",
            Price = 1000000m,
            PropertyType = "House",
            Status = "Available",
            IsFeatured = true,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };
        
        var regularProperty = new Property
        {
            Id = Guid.NewGuid(),
            Title = "Regular Property",
            Description = "Regular Description",
            Address = "123 Regular St",
            City = "Test City",
            State = "Test State",
            ZipCode = "12345",
            Price = 1000000m,
            PropertyType = "House",
            Status = "Available",
            IsFeatured = false,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };
        
        context.Properties.AddRange(featuredProperty, regularProperty);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetFeaturedPropertiesAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().Title.Should().Be("Featured Property");
    }

    [Fact]
    public async Task GetPropertiesByBrokerAsync_ReturnsOnlyBrokerProperties()
    {
        // Arrange
        using var context = new HouseBrokerDbContext(_options);
        var repository = new PropertyRepository(context);
        
        var broker1Property = new Property
        {
            Id = Guid.NewGuid(),
            Title = "Broker 1 Property",
            Description = "Broker 1 Description",
            Address = "123 Broker1 St",
            City = "Test City",
            State = "Test State",
            ZipCode = "12345",
            Price = 1000000m,
            PropertyType = "House",
            Status = "Available",
            BrokerId = "broker-1",
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };
        
        var broker2Property = new Property
        {
            Id = Guid.NewGuid(),
            Title = "Broker 2 Property",
            Description = "Broker 2 Description",
            Address = "123 Broker2 St",
            City = "Test City",
            State = "Test State",
            ZipCode = "12345",
            Price = 1000000m,
            PropertyType = "House",
            Status = "Available",
            BrokerId = "broker-2",
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };
        
        context.Properties.AddRange(broker1Property, broker2Property);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetPropertiesByBrokerAsync("broker-1");

        // Assert
        result.Should().HaveCount(1);
        result.First().Title.Should().Be("Broker 1 Property");
    }
} 