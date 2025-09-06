using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CleanArchitectureApp.Application.Interfaces;
using Microsoft.Extensions.Logging ;

namespace CleanArchitectureApp.Application.Services;

public class BackgroundJobService(
    IProductService productService,
    IEmailService emailService,
    IReportService reportService,
    ILogger<BackgroundJobService> logger) : IBackgroundJobService
{
    private const int LOW_STOCK_THRESHOLD = 10;

    public async Task ProcessProductInventoryUpdateAsync(int productId)
    {
        try
        {
            logger.LogInformation("Starting inventory update process for product {ProductId}", productId);

            var product = await productService.GetProductByIdAsync(productId);
            if (product is null)
            {
                logger.LogWarning("Product {ProductId} not found for inventory update", productId);
                return;
            }

            if (product.Stock <= LOW_STOCK_THRESHOLD)
            {
                await emailService.SendProductLowStockAlertAsync(product.Name, product.Stock);
                logger.LogInformation("Low stock alert sent for product {ProductName}", product.Name);
            }

            logger.LogInformation("Completed inventory update process for product {ProductId}", productId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing inventory update for product {ProductId}", productId);
            throw;
        }
    }

    public async Task SendProductNotificationAsync(int productId, string notificationType)
    {
        try
        {
            logger.LogInformation("Sending {NotificationType} notification for product {ProductId}",
                notificationType, productId);

            var product = await productService.GetProductByIdAsync(productId);
            if (product is null)
            {
                logger.LogWarning("Product {ProductId} not found for notification", productId);
                return;
            }

            var subject = notificationType switch
            {
                "created" => $" New product: {product.Name}",
                "updated" => $"updaed product sucessfully : {product.Name}",
                "deleted" => $"delete product sucessfully  : {product.Name}",
                _ => $"product notificaton for   : {product.Name}"
            };

            var body = $"Done {notificationType} product: {product.Name} price: {product.Price:C}";
            await emailService.SendEmailAsync("admin@example.com", subject, body);

            logger.LogInformation("Notification sent successfully for product {ProductId}", productId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending notification for product {ProductId}", productId);
            throw;
        }
    }

    public async Task GenerateProductReportAsync(DateTime fromDate, DateTime toDate)
    {
        try
        {
            logger.LogInformation("Generating product report from {FromDate} to {ToDate}",
                fromDate.ToShortDateString(), toDate.ToShortDateString());

            var reportData = await reportService.GenerateProductInventoryReportAsync(fromDate, toDate);
            var fileName = $"ProductReport_{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}.xlsx";

            await emailService.SendProductReportAsync("reports@example.com", reportData, fileName);

            logger.LogInformation("Product report generated and sent successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating product report from {FromDate} to {ToDate}",
                fromDate, toDate);
            throw;
        }
    }

    public async Task CleanupOldProductDataAsync()
    {
        try
        {
            logger.LogInformation("Starting cleanup of old product data");

            // clean old adta more tha 2 y
            var cutoffDate = DateTime.UtcNow.AddYears(-2);

            await Task.Delay(1000); 

            logger.LogInformation("Completed cleanup of old product data");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during cleanup of old product data");
            throw;
        }
    }
}