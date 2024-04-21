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

        app.MapPost("/subscription/send", async (IConfiguration configuration, SubscriptionService subscriptionService, ICompareService cs, IFileService fileService) 
                => await SubscriptionHandler.SendEmailsToSubscribed(configuration, subscriptionService, cs, fileService))
            .RequireAuthorization("Admin")
            .WithName("SendSubscription")
            .WithOpenApi();
        app.MapGet("/file/compareFiles", async (ICompareService cs, IFileService fileService) => 
                await FileHandler.CompareFiles(cs, fileService))
            .WithName("CompareFiles")
            .WithOpenApi()
            .RequireAuthorization("Admin");
        app.MapPost("/file/saveReferenceFile", async (IDownloadService ds, IFileService fileService) =>
                await FileHandler.DownloadAndSaveReferenceFile(ds, fileService))
            .WithName("SaveReferenceFile")
            .WithOpenApi()
            .RequireAuthorization("Admin");
        app.MapPost("/file/getDummyReferenceFile", async (IFileService fileService) =>
                await FileHandler.GetDummyReferenceFile())
            .WithName("GetDummyReferenceFile")
            .WithOpenApi()
            .RequireAuthorization("Admin");
        app.Run();
    }
}