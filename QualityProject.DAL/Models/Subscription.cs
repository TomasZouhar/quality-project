using System.ComponentModel.DataAnnotations;

namespace QualityProject.DAL.Models
{
    public class Subscription : BaseModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}
