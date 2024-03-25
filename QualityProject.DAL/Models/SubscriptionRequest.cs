using System.ComponentModel.DataAnnotations;

namespace QualityProject.DAL.Models
{
    public class SubscriptionRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string EmailAddress { get; set; }
    }
}
