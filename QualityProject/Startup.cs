using Microsoft.EntityFrameworkCore;
using QualityProject.API.Handlers;
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

        app.MapGet("/File/CompareFiles", async (IFileService fileService) => await FileHandler.CompareFiles(fileService))
            .RequireAuthorization("Admin");

        app.MapDelete("/subscription/remove", async (string email, SubscriptionService subscriptionService) =>
                await SubscriptionHandler.RemoveSubscriptionAsync(email, subscriptionService))
            .WithName("RemoveSubscription")
            .WithOpenApi();

        app.MapPost("/subscription", async (SubscriptionRequest request, SubscriptionService subscriptionService) =>
                await SubscriptionHandler.AddSubscriptionAsync(request, subscriptionService))
            .WithName("AddSubscription")
            .WithOpenApi();
        
        app.MapDelete("/subscription/removeByEmail", async (string emailAddress, AppDbContext dbContext, HttpContext httpContext) =>
            {
                var subscription = await dbContext.Subscriptions.FirstOrDefaultAsync(s => s.EmailAddress == emailAddress);
                if (subscription == null)
                {
                    return Results.NotFound("Subscription not found.");
                }

                try
                {
                    dbContext.Subscriptions.Remove(subscription);
                    await dbContext.SaveChangesAsync();
                    return Results.Ok("Subscription removed successfully.");
                }
                catch (DbUpdateException ex)
                {
                    return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithName("RemoveSubscriptionByEmail")
            .WithOpenApi();
        
        app.MapDelete("/subscription/removeById", async (long id, AppDbContext dbContext, HttpContext httpContext) =>
            {
                var subscription = await dbContext.Subscriptions.FirstOrDefaultAsync(s => s.Id == id);
                if (subscription == null)
                {
                    return Results.NotFound("Subscription not found.");
                }

                try
                {
                    dbContext.Subscriptions.Remove(subscription);
                    await dbContext.SaveChangesAsync();
                    return Results.Ok("Subscription removed successfully.");
                }
                catch (DbUpdateException ex)
                {
                    return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithName("RemoveSubscriptionByID")
            .WithOpenApi();


        app.MapGet("/subscription", async (SubscriptionService subscriptionService) =>
                await SubscriptionHandler.GetAllSubscriptionAsync(subscriptionService))
            .RequireAuthorization("Admin")
            .WithName("GetSubscriptions")
            .WithOpenApi();

        app.MapPost("/subscription/send", async (IConfiguration configuration, SubscriptionService subscriptionService, IFileService fileService) 
                => await SubscriptionHandler.SendEmailsToSubscribed(configuration, subscriptionService, fileService))
            .RequireAuthorization("Admin")
            .WithName("SendSubscription")
            .WithOpenApi();
        app.Run();
    }
}