using System.Linq;
using Domain.Core;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationServices
{
    internal static class IdentityResultExtensions
    {
        internal static ServiceResult ToServiceResult(this IdentityResult identityResult)
        {
            var errors = identityResult.Errors.Select(identityError => identityError.Description).ToList();
            return new ServiceResult(identityResult.Succeeded, errors);
        }
    }
}
