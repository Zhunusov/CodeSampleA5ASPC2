using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationServices;
using Domain;
using Domain.Identity;
using DumbledoreMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servises.Interfaces;
using Servises.Interfaces.AuthenticationServices;
using Web.Extensions;
using Web.GuidelinesControllers;
using Web.ViewModels.User;

namespace Web.Controllers.Core
{
    /// <summary>
    /// User API.
    /// </summary>
    //[Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller, IFullRestApiController<string, UserPostViewModel, UserPutViewModel>
    {
        private readonly IUserService _userService;

        private readonly IEmailService _emailService;

        private readonly IJwtTokensService _jwtTokensService;

        /// <inheritdoc />
        public UsersController(IUserService userService, IEmailService emailService, IJwtTokensService jwtTokensService)
        {
            _userService = userService;
            _emailService = emailService;
            _jwtTokensService = jwtTokensService;
        }

        /// <summary>
        /// Get count of users.
        /// </summary>
        /// <response code="200">Success.</response>
        [AllowAnonymous]
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> CountAsync()
        {
            return Ok(await _userService.CountAsync());
        }

        /// <summary>
        /// Get list of users.
        /// </summary>
        /// <response code="200">Success.</response>
        /// <response code="400">Failed to get users. Error list in response body.</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<UserGetViewModel>), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> GetAsync(int? count, int? offset = 0)
        {
            var errors = new List<string>();
            if (count < 1)
            {
                errors.Add("Count can not be less than 1.");
            }

            if (offset < 0)
            {
                errors.Add("Offset can not be less than 0.");
            }

            if (errors.Any())
            {
                return BadRequest(errors);
            }

            List<ApplicationUser> users = await _userService.GetListAsync(count, offset);
            var result = Mapper.Map<UserGetViewModel>(users);
            return Ok(result);
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="id">Id of user.</param>
        /// <response code="200">Success.</response>
        /// <response code="400">Failed to get user. Error list in response body.</response>
        /// <response code="404">The user with the received id was not found.</response>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Film), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> GetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new List<string> { "Id cannot be empty" });
            }

            var entity = await _userService.GetByIdOrDefaultAsync(id);

            if (entity == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<UserGetViewModel>(entity));
        }


        /// <summary>
        /// Get user roles.
        /// </summary>
        /// <param name="id">Id of user.</param>
        /// <response code="200">Success.</response>
        /// <response code="400">Failed to get user roles. Error list in response body.</response>
        /// <response code="404">The user with the received id was not found.</response>
        [AllowAnonymous]
        [HttpGet("{id}/roles")]
        [ProducesResponseType(typeof(List<string>), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> GetUserRolesAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new List<string> { "Id cannot be empty" });
            }

            var user = await _userService.GetByIdOrDefaultAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(await _userService.GetRolesAsync(user));
        }

        /// <summary>
        /// Get current user.
        /// </summary>
        /// <response code="200">Success.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="404">User is not found.</response>
        [HttpGet("current")]
        [ProducesResponseType(typeof(UserGetViewModel), 200)]
        public async Task<IActionResult> CurrentAsync()
        {
            var user = await _userService.GetCurrentUserAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<UserGetViewModel>(user));
        }

        /// <summary>
        /// Chang password. In case of success, all user reset tokens will become invalid.
        /// </summary>
        /// <param name="putPasswordViewModel"></param>
        /// <returns></returns>
        /// <response code="200">The user password was changed.</response>
        /// <response code="400">Failed to change password. Error list in response body.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">Access denied. (Attempt to change someone else's account)</response>
        /// <response code="404">User is not found.</response>
        [HttpPut("password")]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> PutPasswordAsync([FromBody]UserPasswordPutViewModel putPasswordViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ErrorsToList());

            var user = await _userService.GetCurrentUserAsync();

            var result = await _userService.ChangePasswordAsync(user, putPasswordViewModel.CurrentPassword, putPasswordViewModel.NewPassword);

            if (result.Succeeded)
            {
                await _jwtTokensService.DeleteUserResetTokensAsync(user);
            }

            return result.ToActionResult();
        }

        /// <summary>
        /// Chang user.
        /// </summary>
        /// <param name="putViewModel"></param>
        /// <response code="200">The user was changed.</response>
        /// <response code="400">Failed to change user. Error list in response body.</response>
        /// <response code="401">User is not authorized.</response>
        [HttpPut]
        [ProducesResponseType(typeof(UserGetViewModel), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> PutAsync([FromBody]UserPutViewModel putViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ErrorsToList());

            var user = await _userService.GetCurrentUserAsync();

            Mapper.CopyProperties(putViewModel, user);

            var currentPasswordCorrect = await _userService.CheckPasswordAsync(user, putViewModel.CurrentPassword);

            if (!currentPasswordCorrect)
            {
                return BadRequest(new List<string> { "Current password is not correct." });
            }

            var result = await _userService.UpdateAsync(user);

            if (result.Succeeded)
            {
                var viewModel = Mapper.Map<UserGetViewModel>(user);
                return Ok(viewModel);
            }

            return result.ToActionResult();
        }

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="id">Id of user to delete.</param>
        /// <response code="200">The user was deleted.</response>
        /// <response code="400">Failed to delete user. Error list in response body.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">You are not the administrator.</response>
        /// <response code="404">The user with the received id was not found.</response>
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new List<string> { "Id cannot be empty" });
            }

            var user = await _userService.GetByIdOrDefaultAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userService.DeleteAsync(user);
            return result.ToActionResult();
        }

        /// <summary>
        /// Create user.
        /// </summary>
        /// <param name="postViewModel"></param>
        /// <response code="201">Success.</response>
        /// <response code="400">Failed to create user. Error list in response body.</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(UserGetViewModel), 201)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> PostAsync([FromBody]UserPostViewModel postViewModel)
        {
            if (!ModelState.IsValid) return new BadRequestObjectResult(ModelState.ErrorsToList());

            var user = Mapper.Map<ApplicationUser>(postViewModel);

            var result = await _userService.CreateAsync(user, postViewModel.Password);

            if (result.Succeeded)
            {
                var viewModel = Mapper.Map<UserGetViewModel>(user);
                return Created(Request.GetDisplayUrl() + "/" + user.Id, viewModel);
            }

            return result.ToActionResult();
        }

        /// <summary>
        /// Reset password. In case of success, all user reset tokens will become invalid.
        /// </summary>
        /// <param name="passwordResertViewModel"></param>
        /// <response code="200">The user password was changed.</response>
        /// <response code="400">Failed to update password. Error list in response body.</response>
        /// <response code="404">The user with the received email was not found.</response>
        [AllowAnonymous]
        [HttpPut("resetPassword")]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] UserPasswordResertViewModel passwordResertViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ErrorsToList());

            var user = await _userService.GetByEmailOrDefaultAsync(passwordResertViewModel.UserEmail);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userService.ResetPasswordAsync(user, passwordResertViewModel.Code, passwordResertViewModel.Password);

            if (result.Succeeded)
            {
                await _jwtTokensService.DeleteUserResetTokensAsync(user);
            }

            return result.ToActionResult();
        }

        /// <summary>
        /// Send reset password url to email addres.
        /// </summary>
        /// <param name="email">Email.</param>
        /// <response code="200">An attempt was made to send the message.</response>
        /// <response code="404">The user with the received email was not found.</response>
        [AllowAnonymous]
        [HttpGet("sendResetPasswordCode")]
        public async Task<IActionResult> SendResetPasswordCodeAsync(string email)
        {
            var user = await _userService.GetByEmailOrDefaultAsync(email);

            if (user == null)
            {
                return NotFound();
            }

            var code = await _userService.GeneratePasswordResetTokenAsync(user);

            //var callbackUrl = HttpContext.Request.Host +
            //                  $"/ResetUserPassword?userId={user.Id}&code={code}";

            await _emailService.SendEmailAsync(user.Email,
                "Reset Password",
                $"Your reset password code: {code}");

            return Ok();
        }

        /// <summary>
        /// Confirm email.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="code">Token of email validation.</param>
        /// <response code="200">Email was confirmed.</response>
        /// <response code="400">Failed to confirm email. Error list in response body.</response>
        /// <response code="404">The user with the received id was not found.</response>
        [AllowAnonymous]
        [HttpGet("confirmEmail")]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string code)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(userId))
            {
                errors.Add("User id is required");
            }

            if (string.IsNullOrEmpty(code))
            {
                errors.Add("User id is required");
            }

            if (errors.Count > 0)
            {
                return BadRequest(errors);
            }

            var user = await _userService.GetByIdOrDefaultAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userService.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Ok("Email confirmed");
            }

            return result.ToActionResult();
        }

        /// <summary>
        /// Send confirm email url to email address of current user.
        /// </summary>
        /// <response code="200">An attempt was made to send the message.</response>
        /// <response code="400">Current user has already verified email.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="404">Current user is not found.</response>
        [HttpGet("sendConfirmEmailCode")]
        public async Task<IActionResult> SendConfirmEmailCodeAsync()
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound();
            }
            if (user.EmailConfirmed)
            {
                return BadRequest("The user has already verified email");
            }

            var code = await _userService.GenerateEmailConfirmationTokenAsync(user);

            var callbackUrl = HttpContext.Request.Host +
                              $"ConfirmEmail?userId={user.Id}&code={code}";

            await _emailService.SendEmailAsync(user.Email,
                "Confirm your email",
                $"Confirm email by clicking on the link: <a href='{callbackUrl}'>link</a>");

            return Ok();
        }
    }
}
