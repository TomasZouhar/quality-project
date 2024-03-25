using System.Net;
using System.Net.Mail;

namespace QualityProject.API.Controller
{
    public static class EmailController
    {
        public static bool SendEmail(IConfiguration configuration, string address, String changes)
        {
            var smtpSettings = configuration.GetSection("SMTP");
            var host = smtpSettings["Host"];
            var port = int.Parse(smtpSettings["Port"]!);
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
                Body = $@"
                <html>
                <head>
                    <style>
                        h1 {{
                            font-size: 24px;
                            color: #333;
                        }}
                        p {{
                            font-size: 16px;
                            color: #666;
                        }}
                        table {{
                            border-collapse: collapse;
                            width: 100%;
                        }}
                        th, td {{
                            border: 1px solid #ddd;
                            padding: 8px;
                            text-align: left;
                        }}
                        th {{
                            background-color: #f2f2f2;
                        }}
                    </style>
                </head>
                <body>
                    <h1>Dear subscriber,</h1>
                    <p>We have detected the following changes in our holdings:</p>
                    {changes}
                    <p>Best regards,</p>
                    <p>Quality Project</p>
                    <p>Date: {DateTime.Now.ToShortDateString()}</p>
                </body>
                </html>",
                IsBodyHtml = true
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