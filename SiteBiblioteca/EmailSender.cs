using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SiteBiblioteca
{
    public class EmailSender : IEmailSender
    {
        public string _smtpUser { get; set; } = "no-replyBibliotecaFixe@outlook.pt";
        public string _smtpPass { get; set; } = "SenhaExtremamenteSegura24/12!";

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.outlook.com", 587)
            {
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress(_smtpUser),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            message.To.Add(email);

            await client.SendMailAsync(message);
        }
    }
}
