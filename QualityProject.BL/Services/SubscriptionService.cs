using Microsoft.EntityFrameworkCore;
using QualityProject.BL.Exceptions;
using QualityProject.DAL;
using QualityProject.DAL.Models;

namespace QualityProject.BL.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly AppDbContext _dbContext;

        public SubscriptionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddSubscriptionAsync(string emailAddress)
        {
            try
            {
                var existingEmailSubscription = await _dbContext.Subscriptions
                    .AnyAsync(s => s.EmailAddress == emailAddress);
                if (existingEmailSubscription)
                {
                    return false;
                }

                var subscription = new Subscription { EmailAddress = emailAddress };
                _dbContext.Subscriptions.Add(subscription);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new CustomException("An error occurred while adding the subscription.", 500, e);
            }
            
        }

        public async Task<bool> RemoveSubscriptionAsync(string emailAddress)
        {
            try
            {
                var subscription = await _dbContext.Subscriptions
                    .FirstOrDefaultAsync(s => s.EmailAddress == emailAddress);
                if (subscription == null)
                {
                    return false;
                }
                _dbContext.Subscriptions.Remove(subscription);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new CustomException("An error occurred while removing the subscription.", 500, e);
            }
            
            return true;
        }

        public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
        {
            try
            { 
                return await _dbContext.Subscriptions.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new CustomException("An error occurred while retrieving all subscriptions.", 500, e);
            }
        }

        public async Task<Subscription> GetSubscriptionByEmailAsync(string emailAddress) => (await _dbContext.Subscriptions
            .FirstOrDefaultAsync(s => s.EmailAddress == emailAddress))!;
    }
}
