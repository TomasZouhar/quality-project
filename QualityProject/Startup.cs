using Microsoft.EntityFrameworkCore;
using QualityProject.API.Handlers;
using QualityProject.DAL;
using QualityProject.DAL.Models;
using QualityProject.BL.Services;

namespace QualityProject;

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

        app.MapGet("/File/CompareFiles", async (ICompareService cs) =>
            {
                var result = await cs.CompareFileAsync();
                return Results.Content(result, "text/plain");
            })
            .RequireAuthorization("Admin");

        app.MapDelete("/subscription/remove", async (string email, SubscriptionService subscriptionService) =>
                await SubscriptionHandler.RemoveSubscriptionAsync(email, subscriptionService))
            .WithName("RemoveSubscription")
            .WithOpenApi();

        app.MapPost("/subscription", async (SubscriptionRequest request, SubscriptionService subscriptionService) =>
                await SubscriptionHandler.AddSubscriptionAsync(request, subscriptionService))
            .WithName("AddSubscription")
            .WithOpenApi();

        app.MapGet("/subscription", async (SubscriptionService subscriptionService) =>
                await SubscriptionHandler.GetAllSubscriptionAsync(subscriptionService))
            .RequireAuthorization("Admin")
            .WithName("GetSubscriptions")
            .WithOpenApi();

        app.MapPost("/subscription/send", async (IConfiguration configuration, SubscriptionService subscriptionService, ICompareService cs) 
                => await SubscriptionHandler.SendEmailsToSubscribed(configuration, subscriptionService, cs))
            .RequireAuthorization("Admin")
            .WithName("SendSubscription")
            .WithOpenApi();
        app.Run();
    }
}