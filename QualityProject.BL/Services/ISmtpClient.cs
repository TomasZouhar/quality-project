using System.Net.Mail;

namespace QualityProject.BL.Services;

public interface ISmtpClient   
{
    void Send(MailMessage mailMessage);
}