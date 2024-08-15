using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using tourismApp.Emailsender;

public class EmailSender : IEmailSender
{
    private readonly SmtpSettings _smtpSettings;

    public EmailSender(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Email address cannot be null or empty.", nameof(email));
        }

        try
        {
            using var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = _smtpSettings.EnableSsl
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.Username, "Sagar Basnet"), 
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            // Log or handle the exception appropriately
            throw new InvalidOperationException("Error sending email.", ex);
        }
    }
}
