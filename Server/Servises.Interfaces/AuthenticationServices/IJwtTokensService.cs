using System.Threading.Tasks;
using Domain.Identity;

namespace Servises.Interfaces.AuthenticationServices
{
    public interface IJwtTokensService
    {
        /// <summary>
        /// Get new jwt token by reset token.
        /// </summary>
        /// <param name="resetToken">Reset token.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation,
        /// containing the Domain.Identity.JwtToken of the operation or null, if the access 
        /// token can not be created.</returns>
        Task<JwtToken> GetJwtTokenByResetTokenAsync(string resetToken);

        /// <summary>
        /// Get a jwt token to authorize the user.
        /// </summary>
        /// <param name="user">The user for whom you want to create a token.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the Domain.Identity.JwtToken of the operation.</returns>
        Task<JwtToken> GetJwtTokenAsync(ApplicationUser user);

        /// <summary>
        /// Block user reset tokens.
        /// </summary>
        /// <param name="user">User whose reset tokens will be invalid.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation.</returns>
        Task DeleteUserResetTokensAsync(ApplicationUser user);
    }
}