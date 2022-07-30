using System.ComponentModel.DataAnnotations;

namespace SSRD.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string? URL { get; set; } = null;
    }
}
