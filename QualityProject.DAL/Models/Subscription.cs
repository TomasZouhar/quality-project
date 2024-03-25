using System.ComponentModel.DataAnnotations;

namespace QualityProject.DAL.Models
{
    public class Subscription : BaseModel
    {
        [Required]
        [MaxLength(63)]
        public required string EmailAddress { get; set; }
    }
}
