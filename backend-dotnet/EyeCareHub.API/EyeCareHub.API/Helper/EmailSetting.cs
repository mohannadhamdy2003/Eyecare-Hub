using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System;

namespace EyeCareHub.API.Helper
{
    public class EmailSetting
    {
        public static async Task SendEmailAsync(string toEmail, string subject, string message)
        {
           
            string fromEmail = "mkemo452@gmail.com";
            string fromPassword = "awjk vyny zxeq cysn"; 

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
