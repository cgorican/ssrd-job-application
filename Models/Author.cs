using System.ComponentModel.DataAnnotations;

namespace SSRD.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? URL { get; set; } = null;
    }
}
