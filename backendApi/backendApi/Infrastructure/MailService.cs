using System.Net.Mail;
using System.Net;

namespace backendApi.Infrastructure
{
    public class MailService : IMailService
    {
        public static string Username { get; set; } = "";
        public static string Password { get; set; } = "";
        public static string SMTPServer { get; set; } = "";
        public static string SMTPPort { get; set; } = "";
        public async Task<bool> SendMailForForgotPassword(string reciepient, string message)
        {
            bool isSent = false;
            SmtpClient smtpClient = new SmtpClient(SMTPServer)
            {
                Port = int.Parse(SMTPPort),
                Credentials = new NetworkCredential(Username, Password),
                EnableSsl = true,
            };
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(Username),
                Subject = "Reset Your Password",
                Body = message,
                IsBodyHtml = true, // Set to true if your email body contains HTML
            };
            mailMessage.To.Add(reciepient); // Replace with the recipient's email address

            try
            {
                smtpClient.Send(mailMessage);
                isSent = true;
            }
            catch (Exception e)
            {
                isSent = false;
                throw e;
            }

            return isSent;
        }

    }

}
