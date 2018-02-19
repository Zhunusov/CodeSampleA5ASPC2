using System.Threading.Tasks;
using Domain.Core;
using Domain.Identity;

namespace Servises.Interfaces.AuthenticationServices
{
    /// <summary>
    /// Provides the APIs for user sign in.
    /// </summary>
    public interface ISignInService
    {
        /// <summary>
        /// Attempts a password sign in for a user.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <param name="lockoutOnFailure">Flag indicating if the user account should be locked if 
        /// the sign in fails.</param>
        /// <returns>The task object representing the asynchronous operation containing 
        /// the Domain.Core.ServiceResult for the sign-in attempt.</returns>
        Task<ServiceResult> CheckPasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure);
    }
}
