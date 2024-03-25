using System.ComponentModel.DataAnnotations;

namespace QualityProject.Web.Models.Subscription
{
    public class SubscriptionViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;
    }
}
