using System.ComponentModel.DataAnnotations;

namespace QualityProject.Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }

    }
}
