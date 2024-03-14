namespace QualityProject
{
    using Microsoft.EntityFrameworkCore;
    using QualityProject.Models;

    public class AppDbContext : DbContext
    {
        public DbSet<SubscriptionModel> Subscriptions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
