using System;
using System.ComponentModel.DataAnnotations;
using Domain.Core.Base;

namespace Domain.Logging
{
    public class ServerEvent: BaseEntity<long>
    {
        public long? EventId { get; set; }

        public DateTime Time { get; set; }

        [Required]
        public int LogLevel { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
