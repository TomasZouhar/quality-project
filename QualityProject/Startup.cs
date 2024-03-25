using Microsoft.EntityFrameworkCore;
using QualityProject.Controller;
using QualityProject.DAL;
using QualityProject.DAL.Models;
using QualityProject.BL.Services;

namespace QualityProject.API;

public static class Startup
{
    public static void Run(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
        }

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGet("/File/CompareFiles", async (IFileService fileService) =>
            {
                var result = await fileService.CompareFileAsync();
                return Results.Content(result, "text/plain");
            })
            .RequireAuthorization("Admin");

        app.MapPost("/subscription", async (SubscriptionRequest request, SubscriptionService subscriptionService, HttpContext httpContext) =>
        {
            var result = await subscriptionService.AddSubscriptionAsync(request.EmailAddress);
            if (!result)
            {
                return Results.Conflict("This email address is already subscribed.");
            }
            var subscription = await subscriptionService.GetSubscriptionByEmailAsync(request.EmailAddress);
            return Results.Created($"/subscribe/{subscription.Id}", subscription);
        })
            .WithName("AddSubscription")
            .WithOpenApi();

        app.MapGet("/subscription", async (SubscriptionService subscriptionService) =>
        {
            var subscriptions = await subscriptionService.GetAllSubscriptionsAsync();
            return Results.Ok(subscriptions);
        })
            .RequireAuthorization("Admin")
            .WithName("GetSubscriptions")
            .WithOpenApi();

        app.MapPost("/subscription/send", async (IConfiguration configuration, SubscriptionService subscriptionService, IFileService fileService) =>
            {
                var smtpSettings = configuration;
                var subscriptions = await subscriptionService.GetAllSubscriptionsAsync();

                var resulBody = await fileService.CompareFileHTMLAsync();

                var sentEmails = 0;

                foreach (var subscription in subscriptions)
                {
                    var sent = EmailController.SendEmail(smtpSettings, subscription.EmailAddress, resulBody);
                    if (sent)
                    {
                        sentEmails++;
                    }
                }

                if (sentEmails == subscriptions.Count())
                {
                    return Results.Ok();
                }
                
                return Results.Problem("Some emails were not sent", statusCode: StatusCodes.Status500InternalServerError);
            })
            .RequireAuthorization("Admin")
            .WithName("SendSubscription")
            .WithOpenApi();
        app.Run();
    }
}