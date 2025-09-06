using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CleanArchitectureApp.Application.DTOs;

namespace CleanArchitectureApp.Application.Interfaces;

public interface IReportService
{
    Task<byte[]> GenerateProductInventoryReportAsync(DateTime fromDate, DateTime toDate);
    Task<ProductReportDto> GetProductStatisticsAsync(int productId);
    Task<IEnumerable<ProductReportDto>> GetLowStockProductsAsync(int threshold);
}