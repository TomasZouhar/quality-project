using System.ComponentModel.DataAnnotations;

namespace QualityProject.Models
{
    public class Subscription : BaseModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}
