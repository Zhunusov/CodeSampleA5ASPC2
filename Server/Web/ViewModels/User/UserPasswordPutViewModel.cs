using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.User
{
    public class UserPasswordPutViewModel
    {
        [MinLength(8)]
        public string NewPassword { get; set; }

        [MinLength(8)]
        public string CurrentPassword { get; set; }
    }
}
