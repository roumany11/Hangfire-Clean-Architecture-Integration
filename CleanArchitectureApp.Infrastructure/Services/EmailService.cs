using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CleanArchitectureApp.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;

namespace CleanArchitectureApp.Infrastructure.Services;

public class EmailService(IConfiguration configuration, ILogger<EmailService> logger) : IEmailService
{
    private readonly string _smtpServer = configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
    private readonly int _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"] ?? "587");
    private readonly string _smtpUsername = configuration["EmailSettings:Username"] ?? "";
    private readonly string _smtpPassword = configuration["EmailSettings:Password"] ?? "";
    private readonly string _fromEmail = configuration["EmailSettings:FromEmail"] ?? "";

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            using var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(_fromEmail, toEmail, subject, body)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(mailMessage);
            logger.LogInformation("Email sent successfully to {ToEmail}", toEmail);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email to {ToEmail}", toEmail);
            throw;
        }
    }

    public async Task SendProductLowStockAlertAsync(string productName, int currentStock)
    {
        var subject = "attention : quantity over  ";
        var body = $@"
            <h2>attention : quantity over</h2>
            <p>product: <strong>{productName}</strong></p>
            <p>current quantity : <strong>{currentStock}</strong></p>
            <p>   pleace increace current quantity recently    .</p>
        ";

        await SendEmailAsync("inventory@example.com", subject, body);
    }

    public async Task SendProductReportAsync(string toEmail, byte[] reportData, string fileName)
    {
        try
        {
            using var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(_fromEmail, toEmail, " Reports products", " attachment of products report ")
            {
                IsBodyHtml = true
            };

            using var stream = new MemoryStream(reportData);
            var attachment = new Attachment(stream, fileName);
            mailMessage.Attachments.Add(attachment);

            await client.SendMailAsync(mailMessage);
            logger.LogInformation("Report email sent successfully to {ToEmail}", toEmail);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send report email to {ToEmail}", toEmail);
            throw;
        }
    }
}