using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Servises.Interfaces.AuthenticationServices;

namespace AuthenticationServices
{
    public class SignInService : ISignInService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SignInService(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<ServiceResult> CheckPasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
            return SingInResultToServiceResult(result);
        }

        protected ServiceResult SingInResultToServiceResult(SignInResult signInResult)
        {
            if (signInResult.Succeeded) return new ServiceResult(true);

            var errors = new List<string>();

            if (signInResult.IsLockedOut) errors.Add("User is lock");

            if (signInResult.IsNotAllowed) errors.Add("User is not allowwed");

            errors.Add("Login failed");

            return new ServiceResult(false, errors);
        }
    }
}
