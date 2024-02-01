#region Usings
using System.Net.Mail;
using System.Net;
#endregion
namespace backendApi.Infrastructure
{
    public class MailService : IMailService
    {
        #region Public variables
        public static string Username { get; set; } = "";
        public static string Password { get; set; } = "";
        public static string SMTPServer { get; set; } = "";
        public static string SMTPPort { get; set; } = "";
        #endregion

        #region Public methods
        /// <summary>
        /// send mail to user to change password
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
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
                IsBodyHtml = true, 
            };
            mailMessage.To.Add(reciepient);

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
        #endregion
    }

}
