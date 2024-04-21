using System.Net;
using System.Net.Mail;

namespace QualityProject.BL.Services;

public class SmtpClientWrapper : ISmtpClient
{
    public SmtpClient SmtpClient { get; set; }
    public SmtpClientWrapper(string host, int port, string username, string password)
    {
        SmtpClient = new SmtpClient(host)
        {
            Port = port,
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true,
        };
    }
    public void Send(MailMessage mailMessage)
    {
        SmtpClient.Send(mailMessage);
    }
}