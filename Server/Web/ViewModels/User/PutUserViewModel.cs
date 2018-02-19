using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.User
{
    public class PutUserViewModel
    {

        public string Id { get; set; }

        [Required]
        [MinLength(4)]
        [RegularExpression("^[0-9]*[A-Za-z][A-Za-z0-9]*$")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(8)]
        public string CurrentPassword { get; set; }

        //public double DateBirthday { get; set; }

        //public string FirstName { get; set; }

        //public string LastName { get; set; }

        //public string AboutMyself { get; set; }
    }
}
