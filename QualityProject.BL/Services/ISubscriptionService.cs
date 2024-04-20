using QualityProject.DAL.Models;

namespace QualityProject.BL.Services
{
    public interface ISubscriptionService
    {
        /// <summary>
        /// Adds a new subscription with the specified email address (if this email address does not already exists in subscriptions)
        /// </summary>
        /// <param name="emailAddress">The email address to be added to active subscriptions</param>
        /// <returns>True if the subscription was successfully added, otherwise returns false</returns>
        Task<bool> AddSubscriptionAsync(string emailAddress);

        /// <summary>
        /// Removes a subscription based on specified email address
        /// </summary>
        /// <param name="emailAddress">The email address of the subscription to remove.</param>
        /// <returns>True if the subscription was successfully removed, otherwise returns false</returns>
        Task<bool> RemoveSubscriptionAsync(string emailAddress);

        /// <summary>
        /// Retrieves all subscriptions that are currently in DB (Subscription table)
        /// </summary>
        /// <returns>A collection of all subscriptions.</returns>
        Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync();

        /// <summary>
        /// Retrieves a subscription by its associated email address.
        /// </summary>
        /// <param name="emailAddress">The email address to find the subscription by</param>
        /// <returns>Subscription if subscription is found based on email address param, otherwise returns false</returns>
        Task<Subscription> GetSubscriptionByEmailAsync(string emailAddress);
    }
}