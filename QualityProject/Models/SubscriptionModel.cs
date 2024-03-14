using System.ComponentModel.DataAnnotations;

namespace QualityProject.Models
{
    public class SubscriptionModel : BaseModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}
