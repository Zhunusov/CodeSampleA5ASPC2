using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Film
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string CoverUrl { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Director { get; set; }

        public int Year { get; set; }
    }
}
