using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Authorization
{
    public class SingInViewModel
    {
        [Required]
        [MinLength(4)]
        public string Username { get; set; }

        [MinLength(8)]
        public string Password { get; set; }
    }
}
