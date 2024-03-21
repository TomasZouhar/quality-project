using Microsoft.EntityFrameworkCore;
using QualityProject.Controller;
using QualityProject.Models;
using QualityProject.Services;

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

        app.MapGet("/File/CompareFiles", async (IFileService fileService) =>
            {
                var result = await fileService.CompareFileAsync();
                return Results.Content(result, "text/plain");
            })
            .RequireAuthorization("Admin");

        app.MapPost("/subscription", async (SubscriptionRequest request, AppDbContext dbContext, HttpContext httpContext) =>
        {
            var existingEmailSubscription = await dbContext.Subscriptions
                                                       .AnyAsync(s => s.EmailAddress == request.EmailAddress);
            if (existingEmailSubscription)
            {
                return Results.Conflict("This email address is already subscribed.");
            }

            var subscription = new Subscription { EmailAddress = request.EmailAddress };

            try
            {
                dbContext.Subscriptions.Add(subscription);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/subscribe/{subscription.Id}", subscription);
            }
            catch (DbUpdateException ex)
            {
                return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
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


        app.MapGet("/subscription", async (AppDbContext dbContext) =>
        {
            var subscriptions = await dbContext.Subscriptions.ToListAsync();
            return Results.Ok(subscriptions);
        })
            .RequireAuthorization("Admin")
            .WithName("GetSubscriptions")
            .WithOpenApi();

        app.MapPost("/subscription/send", async (IConfiguration configuration, AppDbContext dbContext, IFileService fileService) =>
            {
                var smtpSettings = configuration;
                var subscriptions = await dbContext.Subscriptions.ToListAsync();
                
                var resulBody = await fileService.CompareFileReducedAsync();

                var sentEmails = 0;

                foreach (var subscription in subscriptions)
                {
                    var sent = EmailController.SendEmail(smtpSettings, subscription.EmailAddress, resulBody);
                    if (sent)
                    {
                        sentEmails++;
                    }
                }

                if (sentEmails == subscriptions.Count)
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