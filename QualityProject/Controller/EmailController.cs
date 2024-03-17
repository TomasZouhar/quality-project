using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace QualityProject.Controller
{
    public static class EmailController
    {
        public static bool SendEmail(IConfiguration configuration, string address, String changes)
        {
            var smtpSettings = configuration.GetSection("SMTP");
            var host = smtpSettings["Host"];
            var port = int.Parse(smtpSettings["Port"]);
            var username = smtpSettings["Username"];
            var password = smtpSettings["Password"];
            var from = smtpSettings["From"];
            
            if (string.IsNullOrEmpty(host) ||
                port == 0 ||
                string.IsNullOrEmpty(username)||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(from))
            {
                throw new ArgumentNullException();
            }

            var smtpClient = new SmtpClient(host)
            {
                Port = port,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(from),
                Subject = "[QP] Changes in our holdings!",
                Body = @$"
                   Hello, there are new stock changes in our holdings!

                   {changes}

                   Best regards,
                   Quality Project Team
                   
                   Date: {DateTime.Now.ToShortDateString()}
                   "
            };
            mailMessage.To.Add(address);

            try
            {
                smtpClient.Send(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}