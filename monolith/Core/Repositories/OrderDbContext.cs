using Microsoft.EntityFrameworkCore;
using Optivem.EShop.Monolith.Core.Entities;

namespace Optivem.EShop.Monolith.Core.Repositories;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");
            
            entity.HasKey(e => e.OrderNumber);
            
            entity.Property(e => e.OrderNumber)
                .HasColumnName("order_number")
                .IsRequired();

            entity.Property(e => e.OrderTimestamp)
                .HasColumnName("order_timestamp")
                .IsRequired();

            entity.Property(e => e.Country)
                .HasColumnName("country")
                .IsRequired();

            entity.Property(e => e.Sku)
                .HasColumnName("sku")
                .IsRequired();

            entity.Property(e => e.Quantity)
                .HasColumnName("quantity")
                .IsRequired();

            entity.Property(e => e.UnitPrice)
                .HasColumnName("unit_price")
                .HasPrecision(10, 2)
                .IsRequired();

            entity.Property(e => e.OriginalPrice)
                .HasColumnName("original_price")
                .HasPrecision(10, 2)
                .IsRequired();

            entity.Property(e => e.DiscountRate)
                .HasColumnName("discount_rate")
                .HasPrecision(5, 4)
                .IsRequired();

            entity.Property(e => e.DiscountAmount)
                .HasColumnName("discount_amount")
                .HasPrecision(10, 2)
                .IsRequired();

            entity.Property(e => e.SubtotalPrice)
                .HasColumnName("subtotal_price")
                .HasPrecision(10, 2)
                .IsRequired();

            entity.Property(e => e.TaxRate)
                .HasColumnName("tax_rate")
                .HasPrecision(5, 4)
                .IsRequired();

            entity.Property(e => e.TaxAmount)
                .HasColumnName("tax_amount")
                .HasPrecision(10, 2)
                .IsRequired();

            entity.Property(e => e.TotalPrice)
                .HasColumnName("total_price")
                .HasPrecision(10, 2)
                .IsRequired();

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .IsRequired();
        });
    }
}
