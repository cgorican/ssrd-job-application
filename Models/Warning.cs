using System.ComponentModel.DataAnnotations;
using SSRD.Models;

namespace SSRD
{
    public class Warning
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Region { get; set; } = String.Empty;
        [Required]
        public string Severity { get; set; } = String.Empty;
        [Required]
        public DateTime Onset { get; set; } = DateTime.Now;

        public int? AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}
