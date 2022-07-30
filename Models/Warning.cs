using System.ComponentModel.DataAnnotations;

namespace SSRD
{
    public class Warning
    {
        public int Id { get; set; }
        [Required]
        public string AreaDesc { get; set; } = String.Empty;
        [Required]
        public DateTime Onset { get; set; }
        [Required]
        public string Severity { get; set; } = String.Empty;
        [Required]
        public int? AuthorId { get; set; }
    }
}
