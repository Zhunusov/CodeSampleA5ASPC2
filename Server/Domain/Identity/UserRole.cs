using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class UserRole: IdentityRole
    {
        public UserRole(string roleName) : base(roleName) { }
        public UserRole() { }
    }
}
