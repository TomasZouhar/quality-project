using Microsoft.AspNetCore.Mvc;
using QualityProject.BL.Services;
using QualityProject.Web.Models.Subscription;

namespace QualityProject.Web.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly SubscriptionService _subscriptionService;

        public SubscriptionController(SubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var subscriptions = await _subscriptionService.GetAllSubscriptionsAsync();
            return View(subscriptions);
        }

        [HttpGet]
        public IActionResult AddSubscription()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSubscription(SubscriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _subscriptionService.AddSubscriptionAsync(model.Email);
                if (result)
                {
                    TempData["Success"] = "Subscription added successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Email is already subscribed.";
                    ModelState.AddModelError("", "Email is already subscribed.");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveSubscription(string email)
        {
            var result = await _subscriptionService.RemoveSubscriptionAsync(email);
            if (result)
            {
                TempData["Success"] = "Subscription removed successfully.";
            }
            else
            {
                TempData["Error"] = "Subscription removal failed.";
            }
            return RedirectToAction("Index");
        }
    }
}
