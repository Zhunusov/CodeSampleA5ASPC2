using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servises.Interfaces.AuthenticationServices;
using Web.Extensions;
using Web.ViewModels.Authorization;

namespace Web.Controllers.Core
{
    /// <inheritdoc />
    [Authorize]
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly ISignInService _signInService;

        private readonly IUserService _userService;

        private readonly IJwtTokensService _jwtTokenServices;

        /// <inheritdoc />
        public AuthorizationController(
            IJwtTokensService jwtTokenServices, ISignInService signInService, IUserService userService)
        {
            _jwtTokenServices = jwtTokenServices;
            _signInService = signInService;
            _userService = userService;
        }

        /// <summary>
        /// Get jwt token.
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">Success.</response>
        /// <response code="400">Invalid password.</response>
        /// <response code="404">User with received email or name not found.</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(JwtToken), 200)]
        public async Task<IActionResult> Post([FromBody]SingInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ErrorsToList());
            }

            var normalizedUserName = model.Username.Trim().ToUpper();

            var user = await _userService.GetByUserNameOrEmailOrDefaultAsync(normalizedUserName);

            if (user == null)
            {
                return NotFound($"User with email or name {model.Username} not found");
            }

            var result = await _signInService.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded) return result.ToActionResult();

            return Ok(await _jwtTokenServices.GetJwtTokenAsync(user));
        }

        /// <summary>
        /// Get new jwt access token by reset token.
        /// </summary>
        /// <param name="model">Reset jwt token.</param>
        /// <response code="200">New access token.</response>
        /// <response code="400">Failed to get new access jwt token.</response>
        [AllowAnonymous]
        [ProducesResponseType(typeof(JwtToken), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        [HttpPost("UpdateAccessJwtToken")]
        public async Task<IActionResult> UpdateAccessJwtToken([FromBody]ResetTokenViewModel model)
        {
            var token = await _jwtTokenServices.GetJwtTokenByResetTokenAsync(model.ResetToken);
            if (token == null)
            {
                return BadRequest();
            }

            return Ok(token);
        }
    }
}
