using Microsoft.EntityFrameworkCore;
using QualityProject.API.Handlers;
using QualityProject.DAL;
using QualityProject.DAL.Models;
using QualityProject.BL.Services;
using QualityProject.API.Enums;
using Microsoft.AspNetCore.Mvc;
using FileResult = QualityProject.DAL.Models.FileResult;

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

        app.MapPost("/subscription/send", async (IConfiguration configuration, SubscriptionService subscriptionService, ICompareService cs, IFileService fileService, IFormatService formatService, IDownloadService downloadService) 
                => await SubscriptionHandler.SendEmailsToSubscribed(configuration, subscriptionService, cs, fileService, downloadService, formatService))
            .RequireAuthorization("Admin")
            .WithName("SendSubscription")
            .WithOpenApi();
        app.MapGet("/file/compareFiles", async (ICompareService cs, IFileService fileService, IFormatService formatService, IDownloadService downloadService) => 
                await FileHandler.CompareFiles(cs, fileService, formatService, downloadService))
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

        app.MapGet("/file/stockChange", async (ICompareService cs,
                                                IFileGenerationService fileGenerationService,
                                                IFormatService formatService,
                                                IDownloadService downloadService,
                                                IFileService fileService,
                                                [FromQuery] FileType fileType) =>
        {
            FileResult fileResult = new();
            var referenceFile = fileService.GetFileFromDisk("referenceFile.csv");
            var downloadedFile = await downloadService.DownloadFileAsync();
            var difference = await cs.CompareFilesStringAsync(downloadedFile, referenceFile);
            switch (fileType)
            {
                case FileType.Csv:
                    var csvContent = formatService.FormatHoldingsTable(difference);
                    fileResult = await fileGenerationService.GenerateCsvFileAsync(csvContent);
                    break;
                case FileType.Pdf:
                    var pdfHtmlContent = formatService.FormatHTMLHoldingsTable(difference);
                    fileResult = await fileGenerationService.GeneratePdfFileAsync(pdfHtmlContent);
                    break;
                case FileType.Html:
                    var htmlContent = formatService.FormatHTMLHoldingsTable(difference);
                    fileResult = await fileGenerationService.GenerateHtmlFileAsync(htmlContent);
                    break;
                default:
                    return Results.BadRequest("Invalid file type.");
            }
            return Results.File(fileResult.FileContent, fileResult.ContentType, fileResult.FileName);
        })
        .RequireAuthorization("Admin")
        .WithName("GetSubscriptionFile")
        .WithOpenApi();

        app.Run();
    }
}