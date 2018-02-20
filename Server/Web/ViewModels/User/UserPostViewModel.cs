using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.User
{
    public class UserPostViewModel
    {
        [Required]
        [MinLength(4)]
        [RegularExpression("^[0-9]*[A-Za-z][A-Za-z0-9]*$")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
