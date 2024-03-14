using Microsoft.EntityFrameworkCore;
using QualityProject;
using QualityProject.Controller;
using QualityProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configure dbcontext

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapPost("/subscribe", async (SubscriptionRequest request, AppDbContext dbContext, HttpContext httpContext) =>
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

app.MapGet("/subscriptions", async (AppDbContext dbContext) =>
{
    var subscriptions = await dbContext.Subscriptions.ToListAsync();
    return Results.Ok(subscriptions);
})
    .WithName("GetSubscriptions")
    .WithOpenApi();

app.MapPost("/sendSubscription", async (AppDbContext dbContext) =>
    {
        var subscriptions = await dbContext.Subscriptions.ToListAsync();

        foreach (var subscription in subscriptions)
        {
            EmailController.SendEmail(subscription.EmailAddress);
        }
    
        return Results.Ok(subscriptions);
    })
    .WithName("SendSubscription")
    .WithOpenApi();

app.Run();



record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}