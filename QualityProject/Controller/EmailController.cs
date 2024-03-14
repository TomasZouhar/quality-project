using System.Net;
using System.Net.Mail;

namespace QualityProject.Controller;

public static class EmailController
{
    public static bool SendEmail(String address)
    {
        // Send email via SMTP
        var smtpClient = new SmtpClient("smtp.endora.cz")
        {
            Port = 587,
            Credentials = new NetworkCredential("info@tomaszouhar.cz", "HesloHeslo123"),
            EnableSsl = true,
        };
        
        var mailMessage = new MailMessage
        {
            From = new MailAddress("quality@project.cz"),
            Subject = "[QP] New Stock Difference",
            Body = """
                   Hello, there are new stock changes in our company!
                   Second Line.
                   """
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