namespace QualityProject.DAL
{
    using Microsoft.EntityFrameworkCore;
    using QualityProject.DAL.Models;

    public class AppDbContext : DbContext
    {
        public DbSet<Subscription> Subscriptions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
