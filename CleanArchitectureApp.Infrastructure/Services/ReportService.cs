using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces;
using CleanArchitectureApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureApp.Infrastructure.Services;

public class ReportService(ApplicationDbContext context, ILogger<ReportService> logger) : IReportService
{
    public async Task<byte[]> GenerateProductInventoryReportAsync(DateTime fromDate, DateTime toDate)
    {
        try
        {
            var products = await context.Products
                .Where(p => !p.IsDeleted && p.CreatedDate >= fromDate && p.CreatedDate <= toDate)
                .OrderBy(p => p.Name)
                .ToListAsync();

            // محاكاة إنشاء ملف Excel
            var reportContent = $"Product Inventory Report\nFrom: {fromDate:yyyy-MM-dd}\nTo: {toDate:yyyy-MM-dd}\n\n";
            reportContent += "ID,Name,Description,Price,Stock,Active,Created\n";

            foreach (var product in products)
            {
                reportContent += $"{product.Id},{product.Name},{product.Description},{product.Price},{product.Stock},{product.IsActive},{product.CreatedDate:yyyy-MM-dd}\n";
            }

            return System.Text.Encoding.UTF8.GetBytes(reportContent);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating product inventory report");
            throw;
        }
    }

    public async Task<ProductReportDto> GetProductStatisticsAsync(int productId)
    {
        var product = await context.Products
            .Where(p => p.Id == productId && !p.IsDeleted)
            .FirstOrDefaultAsync();

        if (product is null)
            throw new ArgumentException($"Product with ID {productId} not found");

        return new ProductReportDto
        {
            Id = product.Id,
            Name = product.Name,
            CurrentStock = product.Stock,
            Price = product.Price,
            LastUpdated = product.UpdatedDate ?? product.CreatedDate,
            IsLowStock = product.Stock <= 10,
            ReorderLevel = 20
        };
    }

    public async Task<IEnumerable<ProductReportDto>> GetLowStockProductsAsync(int threshold)
    {
        var products = await context.Products
            .Where(p => !p.IsDeleted && p.Stock <= threshold)
            .OrderBy(p => p.Stock)
            .ToListAsync();

        return products.Select(p => new ProductReportDto
        {
            Id = p.Id,
            Name = p.Name,
            CurrentStock = p.Stock,
            Price = p.Price,
            LastUpdated = p.UpdatedDate ?? p.CreatedDate,
            IsLowStock = true,
            ReorderLevel = threshold * 2
        });
    }
}