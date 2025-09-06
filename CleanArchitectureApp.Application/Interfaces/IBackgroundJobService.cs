using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureApp.Application.Interfaces;

public interface IBackgroundJobService
{
    Task ProcessProductInventoryUpdateAsync(int productId);
    Task SendProductNotificationAsync(int productId, string notificationType);
    Task GenerateProductReportAsync(DateTime fromDate, DateTime toDate);
    Task CleanupOldProductDataAsync();
}