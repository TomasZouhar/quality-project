using System.Net;
using System.Net.Mail;
using QualityProject.API.Controller;
using QualityProject.BL.Services;
using QualityProject.DAL.Models;


namespace QualityProject.API.Handlers;

public static class SubscriptionHandler
{
    
    public static async Task<IResult> RemoveSubscriptionAsync(string emailAddress, ISubscriptionService subscriptionService)
    {
       var result = await subscriptionService.RemoveSubscriptionAsync(emailAddress);
       
       return result ? Results.Ok(result) : Results.NotFound(result);
    }

    public static async Task<IResult> GetAllSubscriptionAsync(ISubscriptionService subscriptionService)
    {
        var result = await subscriptionService.GetAllSubscriptionsAsync();
        return Results.Ok(result);
    }

    public static async Task<IResult> AddSubscriptionAsync(SubscriptionRequest request, ISubscriptionService subscriptionService)
    {
        var result = await subscriptionService.AddSubscriptionAsync(request.EmailAddress);
        var subscription = await subscriptionService.GetSubscriptionByEmailAsync(request.EmailAddress);
        return result ? Results.Created($"/subscribe/{subscription.Id}", subscription) : Results.Conflict("This email address is already subscribed.");
    }

    public static async Task<IResult> SendEmailsToSubscribed(IConfiguration smtpConfiguration, ISubscriptionService subscriptionService, ICompareService cs)
    {
        var subscriptions = (await subscriptionService.GetAllSubscriptionsAsync()).ToList();
        
        var smtpSettings = smtpConfiguration.GetSection("SMTP");
        var host = smtpSettings["Host"];
        var port = int.Parse(smtpSettings["Port"]!);
        var username = smtpSettings["Username"];
        var password = smtpSettings["Password"];
        
        var resultBody = await cs.CompareFileHTMLAsync();
        
        if (string.IsNullOrEmpty(host) ||
            port == 0 ||
            string.IsNullOrEmpty(username)||
            string.IsNullOrEmpty(password))
        {
            throw new ArgumentNullException();
        }
        
        var smtpClient = new SmtpClientWrapper(host, port, username, password);

        
        var sentEmails = subscriptions.Select(subscription => EmailController.SendEmail(smtpConfiguration, subscription.EmailAddress, resultBody, smtpClient)).Count(sent => sent);

        var allEmailsSent = sentEmails == subscriptions.Count();
        return allEmailsSent ? Results.Ok() : 
            Results.Problem("Some emails were not sent", statusCode: StatusCodes.Status500InternalServerError);
    }
}