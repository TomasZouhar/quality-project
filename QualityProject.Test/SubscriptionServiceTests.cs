
using Microsoft.EntityFrameworkCore;
using QualityProject.BL.Services;
using QualityProject.DAL;
using QualityProject.DAL.Models;

namespace QualityProject.Test
{

    public class SubscriptionServiceTests : IDisposable
    {
        private DbContextOptions<AppDbContext> _dbContextOptions;
        private AppDbContext _dbContext;
        private SubscriptionService _subscriptionService;


        public SubscriptionServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "SubscriptionServiceTestsDB")
                .Options;
            _dbContext = new AppDbContext(_dbContextOptions);

            _subscriptionService = new SubscriptionService(_dbContext);

            InitializeDbForTests(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        private void InitializeDbForTests(AppDbContext dbContext)
        {
            // reset database between test runs
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
            SeedInitialSubscriptions(dbContext);
        }


        private void SeedInitialSubscriptions(AppDbContext dbContext)
        {
            dbContext.Subscriptions.AddRange(
                new Subscription { EmailAddress = "user1@example.com" },
                new Subscription { EmailAddress = "user2@example.com" }
            );
            dbContext.SaveChanges();
        }

        [Fact]
        public async void AddSubscription_Success()
        {
            // Arrange
            var emailAddress = "user3@example.com";

            // Act
            var result = await _subscriptionService.AddSubscriptionAsync(emailAddress);
            var dbResult = await _dbContext.Subscriptions.AnyAsync(s => s.EmailAddress == emailAddress);

            // Assert
            Assert.True(result);
            Assert.True(dbResult);

        }

    }

}