using HouseBroker.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Data;

public class HouseBrokerDbContext : IdentityDbContext<ApplicationUser>
{
    public HouseBrokerDbContext(DbContextOptions<HouseBrokerDbContext> options) : base(options)
    {
    }

    public DbSet<Property> Properties { get; set; }
    public DbSet<CommissionRate> CommissionRates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ApplicationUser configuration
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Bio).HasMaxLength(500);
            entity.Property(e => e.ProfileImageUrl).HasMaxLength(255);
        });

        // Property configuration
        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Address).IsRequired().HasMaxLength(200);
            entity.Property(e => e.City).IsRequired().HasMaxLength(100);
            entity.Property(e => e.State).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ZipCode).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.PropertyType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Features).HasMaxLength(500);
            entity.Property(e => e.MainImageUrl).HasMaxLength(255);
        });



        // CommissionRate configuration
        modelBuilder.Entity<CommissionRate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MinPrice).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.MaxPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.RatePercentage).IsRequired().HasColumnType("decimal(5,2)");
            entity.Property(e => e.Description).IsRequired().HasMaxLength(100);

            // Seed default commission rates
            entity.HasData(
                new CommissionRate
                {
                    Id = new Guid("11111111-1111-1111-1111-111111111111"),
                    MinPrice = 0m,
                    MaxPrice = 5000000m,
                    RatePercentage = 2.0m,
                    Description = "2% for price < 50,00,000",
                    IsActive = true
                },
                new CommissionRate
                {
                    Id = new Guid("22222222-2222-2222-2222-222222222222"),
                    MinPrice = 5000000m,
                    MaxPrice = 10000000m,
                    RatePercentage = 1.75m,
                    Description = "1.75% for 50,00,000 <= price <= 1 crore",
                    IsActive = true
                },
                new CommissionRate
                {
                    Id = new Guid("33333333-3333-3333-3333-333333333333"),
                    MinPrice = 10000000m,
                    MaxPrice = null,
                    RatePercentage = 1.5m,
                    Description = "1.5% for price > 1 crore",
                    IsActive = true
                }
            );
        });
    }
}