using QualityProject.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QualityProject.BL.Services
{
    public interface ISubscriptionService
    {
        Task<bool> AddSubscriptionAsync(string emailAddress);
        Task<bool> RemoveSubscriptionAsync(string emailAddress);
        Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync();
        Task<Subscription> GetSubscriptionByEmailAsync(string emailAddress);
    }
}