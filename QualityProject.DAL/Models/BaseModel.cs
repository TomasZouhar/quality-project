using System.ComponentModel.DataAnnotations;

namespace QualityProject.DAL.Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }

    }
}
