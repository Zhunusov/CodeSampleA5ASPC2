using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.User
{
    public class UserPutViewModel
    {
        [Required]
        [MinLength(4)]
        [RegularExpression("^[0-9]*[A-Za-z][A-Za-z0-9]*$")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(8)]
        public string CurrentPassword { get; set; }
    }
}
