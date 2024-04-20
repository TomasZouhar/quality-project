using System.Net.Mail;
using Moq;
using Microsoft.Extensions.Configuration;
using QualityProject.API.Controller;
using QualityProject.BL.Services;

namespace QualityProject.Test
{
    public class EmailServiceTests
    {
        [Fact]
        public void SendEmail_Success()
        {
            // Arrange
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c.GetSection("SMTP")["Host"]).Returns("smtp.example.com");
            configuration.Setup(c => c.GetSection("SMTP")["Port"]).Returns("587");
            configuration.Setup(c => c.GetSection("SMTP")["Username"]).Returns("testuser");
            configuration.Setup(c => c.GetSection("SMTP")["Password"]).Returns("testpassword");
            configuration.Setup(c => c.GetSection("SMTP")["From"]).Returns("test@example.com");

            var address = "recipient@example.com";
            var changes = "Test changes";
            
            var smtpClient = new Mock<ISmtpClient>();
            smtpClient.Setup(c => c.Send(It.IsAny<MailMessage>()));
            
            // Act
            var result = EmailController.SendEmail(configuration.Object, address, changes, smtpClient.Object);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SendEmail_ConfigurationMissing()
        {
            // Arrange
            var configuration = new Mock<IConfiguration>();
            var smtpClient = new Mock<ISmtpClient>();
            smtpClient.Setup(c => c.Send(It.IsAny<MailMessage>()));

            var address = "recipient@example.com";
            var changes = "Test changes";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => EmailController.SendEmail(configuration.Object, address, changes, smtpClient.Object));
        }

        [Fact]
        public void SendEmail_Failure()
        {
            // Arrange
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c.GetSection("SMTP")["Host"]).Returns("smtp.example.com");
            configuration.Setup(c => c.GetSection("SMTP")["Port"]).Returns("587");
            configuration.Setup(c => c.GetSection("SMTP")["Username"]).Returns("testuser");
            configuration.Setup(c => c.GetSection("SMTP")["Password"]).Returns("testpassword");
            configuration.Setup(c => c.GetSection("SMTP")["From"]).Returns("test@example.com");
            
            var smtpClient = new Mock<ISmtpClient>();
            smtpClient.Setup(c => c.Send(It.IsAny<MailMessage>())).Throws(new SmtpException());

            var address = "invalid@example.com";
            var changes = "Test changes";

            // Act
            var result = EmailController.SendEmail(configuration.Object, address, changes, smtpClient.Object);

            // Assert
            Assert.False(result);
        }
    }
}
