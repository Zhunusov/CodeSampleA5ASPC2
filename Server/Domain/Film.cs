using System.ComponentModel.DataAnnotations;
using Domain.Core.Base;

namespace Domain
{
    public class Film: BaseEntity<int>
    {
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
