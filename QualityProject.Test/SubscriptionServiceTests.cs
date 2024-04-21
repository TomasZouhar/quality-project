
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
        private List<Subscription> _seedSubscriptions;



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
            _seedSubscriptions = new List<Subscription>
            {
                new Subscription { EmailAddress = "user1@example.com" },
                new Subscription { EmailAddress = "user2@example.com" }
            };

            dbContext.Subscriptions.AddRange(_seedSubscriptions);
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

        [Fact]
        public async void AddSubscription_DuplicateFailure()
        {
            // Arrange
            var emailAddress = "user2@example.com";

            // Act
            var result = await _subscriptionService.AddSubscriptionAsync(emailAddress);
            var dbResult = await _dbContext.Subscriptions.AnyAsync(s => s.EmailAddress == emailAddress);

            // Assert
            Assert.False(result);
            Assert.True(dbResult);
        }


        [Fact]
        public async void RemoveSubscription_Success()
        {
            // Arrange
            var emailAddress = "user1@example.com";

            // Act
            var result = await _subscriptionService.RemoveSubscriptionAsync(emailAddress);
            var dbResult = await _dbContext.Subscriptions.AnyAsync(s => s.EmailAddress == emailAddress);

            // Assert
            Assert.True(result);
            Assert.False(dbResult);
        }


        [Fact]
        public async void RemoveNonExistingSubscription_Failure()
        {
            // Arrange
            var emailAddress = "user44@example.com";

            // Act
            var result = await _subscriptionService.RemoveSubscriptionAsync(emailAddress);
            var dbResult = await _dbContext.Subscriptions.AnyAsync(s => s.EmailAddress == emailAddress);

            // Assert
            Assert.False(result);
            Assert.False(dbResult);
        }

        [Fact]
        public async void GetAllSubscriptionsTest()
        {
            // Act
            var subscriptions = await _subscriptionService.GetAllSubscriptionsAsync();

            // Assert
            var seedEmails = _seedSubscriptions.Select(s => s.EmailAddress).ToList();
            var dbEmails = subscriptions.Select(s => s.EmailAddress).ToList();
            Assert.Equal(dbEmails.Count(), seedEmails.Count());
            Assert.True(seedEmails.All(e => dbEmails.Contains(e)), "All seed subscriptions should be retrieved from DB.");
        }

        [Fact]
        public async void GetSubscriptionByEmail_Success()
        {
            // Arrange
            var emailAddress = "user1@example.com";

            // Act
            var subscription = await _subscriptionService.GetSubscriptionByEmailAsync(emailAddress);

            // Assert
            Assert.NotNull(subscription);
            Assert.Equal(emailAddress, subscription.EmailAddress);

        }

        [Fact]
        public async void GetSubscriptionByNonExistingEmailTest()
        {
            // Arrange
            var emailAddress = "nonexistinguser44@example.com";

            // Act
            var subscription = await _subscriptionService.GetSubscriptionByEmailAsync(emailAddress);

            // Assert
            Assert.Null(subscription);

        }

    }

}