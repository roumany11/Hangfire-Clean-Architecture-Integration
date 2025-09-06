using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureApp.Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
    Task SendProductLowStockAlertAsync(string productName, int currentStock);
    Task SendProductReportAsync(string toEmail, byte[] reportData, string fileName);
}
