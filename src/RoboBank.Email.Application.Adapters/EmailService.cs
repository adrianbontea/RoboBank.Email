using System.Net.Mail;
using System.Threading.Tasks;
using RoboBank.Email.Application.Ports;

namespace RoboBank.Email.Application.Adapters
{
    public class EmailService : IEmailService
    {
        public async Task SendAsync(string to, string subject, string message)
        {
            var mailMessage = new MailMessage
            {
                Subject = subject,
                Body = message
            };

            mailMessage.To.Add(new MailAddress(to));

            using (var client = new SmtpClient
            {
                UseDefaultCredentials = false,
                EnableSsl = true
            })
            {
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
