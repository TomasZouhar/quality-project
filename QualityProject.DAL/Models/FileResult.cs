using System.ComponentModel.DataAnnotations;

namespace QualityProject.DAL.Models
{
    public class FileResult
    {
        public byte[] FileContent { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public string StringContent { get; set; }
    }
}
