using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Logging
{
    public class ServerEvent
    {
        [Key]
        public long Id { get; set; }

        public int? EventId { get; set; }

        public DateTime Time { get; set; }

        [Required]
        public int LogLevel { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
