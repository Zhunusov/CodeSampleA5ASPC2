using Domain.Core.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class ApplicationUser : IdentityUser, IBaseEntity<string>
    {

    }
}
