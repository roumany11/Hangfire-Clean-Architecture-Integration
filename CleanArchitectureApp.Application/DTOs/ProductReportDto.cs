using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureApp.Application.DTOs;

public class ProductReportDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
    public decimal Price { get; set; }
    public DateTime LastUpdated { get; set; }
    public bool IsLowStock { get; set; }
    public int ReorderLevel { get; set; }
}