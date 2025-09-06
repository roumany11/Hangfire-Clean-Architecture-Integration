using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

// CleanArchitectureApp.Infrastructure/Data/ApplicationDbContext.cs
using CleanArchitectureApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.HasIndex(e => e.Name);
        });

        // Seed Data
        modelBuilder.Entity<Product>().HasData(
        [
            new Product
            {
                Id = 1,
                Name = " product1",
                Description = " description",
                Price = 100.50m,
                Stock = 10,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            },
            new Product
            {
                Id = 2,
               Name = " product2",
                Description = " description",
                Price = 250.75m,
                Stock = 5,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            }
        ]);

        base.OnModelCreating(modelBuilder);
    }
}