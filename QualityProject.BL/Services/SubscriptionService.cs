using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QualityProject.DAL;
using QualityProject.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<bool> RemoveSubscriptionAsync(string emailAddress)
        {
            var subscription = await _dbContext.Subscriptions
                                               .FirstOrDefaultAsync(s => s.EmailAddress == emailAddress);
            if (subscription == null)
            {
                return false;
            }
            _dbContext.Subscriptions.Remove(subscription);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
        {
            return await _dbContext.Subscriptions.ToListAsync();
        }

        public async Task<Subscription> GetSubscriptionByEmailAsync(string emailAddress) => await _dbContext.Subscriptions
                                   .FirstOrDefaultAsync(s => s.EmailAddress == emailAddress);
    }
}
