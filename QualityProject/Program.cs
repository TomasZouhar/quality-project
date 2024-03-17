using QualityProject.Services;
using Microsoft.EntityFrameworkCore;
using QualityProject;
using QualityProject.Controller;
using QualityProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFileService, FileService>();

// configure dbcontext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/File/CompareFiles", async (IFileService fileService) =>
{
    var result = await fileService.CompareFileAsync();
    return Results.Content(result, "text/plain");
});

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

app.MapGet("/subscription", async (AppDbContext dbContext) =>
{
    var subscriptions = await dbContext.Subscriptions.ToListAsync();
    return Results.Ok(subscriptions);
})
    .WithName("GetSubscriptions")
    .WithOpenApi();

app.MapPost("/subscription/send", async (IConfiguration configuration, AppDbContext dbContext) =>
    {
        var smtpSettings = configuration;
        var subscriptions = await dbContext.Subscriptions.ToListAsync();

        foreach (var subscription in subscriptions)
        {
            EmailController.SendEmail(smtpSettings, subscription.EmailAddress);
        }
    
        return Results.Ok(subscriptions);
    })
    .WithName("SendSubscription")
    .WithOpenApi();

app.Run();
