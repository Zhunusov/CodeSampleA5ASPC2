using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.User
{
    public class ResertUserPasswordViewModel
    {
        [Required]
        public string UserEmail { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
