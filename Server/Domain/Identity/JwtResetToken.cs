using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Identity
{
    public class JwtResetToken
    {
        [Key]
        public string ResetToken { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string ClientIp { get; set; }

        [Required]
        public string UserAgent { get; set; }

        [Required]
        public DateTime CreateDateTime { get; set; }
    }
}
