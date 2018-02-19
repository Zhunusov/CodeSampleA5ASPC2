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
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller, IFullRestApiController<string, PostUserViewModel, PutUserViewModel>
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
        /// Gel count of users.
        /// </summary>
        /// <response code="200">Success.</response>
        [AllowAnonymous]
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> CountAsync()
        {
            return Ok(await _userService.Users.CountAsync());
        }

        /// <summary>
        /// Gel list of users.
        /// </summary>
        /// <response code="200">Success.</response>
        /// <response code="400">Failed to get users. Error list in response body.</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<GetUserViewModel>), 200)]
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

            List<ApplicationUser> users;
            if (count == null)
            {
                users = await _userService.Users.Skip(offset.GetValueOrDefault()).ToListAsync();
            }
            else
            {
                users = await _userService.Users.Skip(offset.GetValueOrDefault()).Take(count.Value).ToListAsync();
            }

            var result = Mapper.Map<GetUserViewModel>(users);
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

            var entity = await _userService.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (entity == null)
            {
                return NotFound();
            }
            return Ok(Mapper.Map<GetUserViewModel>(entity));
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

            var user = await _userService.Users.FirstOrDefaultAsync(u => u.Id == id);
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
        [ProducesResponseType(typeof(GetUserViewModel), 200)]
        public async Task<IActionResult> CurrentAsync()
        {
            var user = await GetCurrentUserAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<GetUserViewModel>(user));
        }

        /// <summary>
        /// Chang password. In case of success, all user reset tokens will become invalid.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="200">The user password was changed.</response>
        /// <response code="400">Failed to change password. Error list in response body.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">Access denied. (Attempt to change someone else's account)</response>
        /// <response code="404">User is not found.</response>
        [HttpPut("password")]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> PutPasswordAsync([FromBody]PutUserPasswordViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ErrorsToList());

            var user = await GetCurrentUserAsync();

            var result = await _userService.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                await _jwtTokensService.DeleteUserResetTokensAsync(user);
            }

            return result.ToActionResult();
        }

        /// <summary>
        /// Chang user. If the user is not an administrator, then he can change only the current account.
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">The user was changed.</response>
        /// <response code="400">Failed to change user. Error list in response body.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">Access denied. (Attempt to change someone else's account)</response>
        /// <response code="404">User is not found.</response>
        [HttpPut]
        [ProducesResponseType(typeof(GetUserViewModel), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> PutAsync([FromBody]PutUserViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ErrorsToList());

            var user = await GetCurrentUserAsync();
            if (user.Id != model.Id)
            {
                return StatusCode(403);
            }

            user = await _userService.Users.FirstOrDefaultAsync(u => u.Id == model.Id);

            if (user == null)
            {
                return NotFound();
            }

            Mapper.CopyProperties(model, user);

            var currentPasswordCorrect = await _userService.CheckPasswordAsync(user, model.CurrentPassword);

            if (!currentPasswordCorrect)
            {
                return BadRequest(new List<string>{"Current password is not correct"});
            }

            var result = await _userService.UpdateAsync(user);

            if (result.Succeeded)
            {
                var viewModel = Mapper.Map<GetUserViewModel>(user);
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
                return BadRequest(new List<string> { "Id cannot be empty"});
            }

            var user = await _userService.Users.FirstOrDefaultAsync(u => u.Id == id);
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
        /// <param name="model"></param>
        /// <response code="201">Success.</response>
        /// <response code="400">Failed to create user. Error list in response body.</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(GetUserViewModel), 201)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> PostAsync([FromBody]PostUserViewModel model)
        {
            if (!ModelState.IsValid) return new BadRequestObjectResult(ModelState.ErrorsToList());

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Username
            };

            var result = await _userService.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Created(Request.GetDisplayUrl() + "/" + user.Id, user);
            }

            return result.ToActionResult();
        }

        /// <summary>
        /// Reset password. In case of success, all user reset tokens will become invalid.
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">The user password was changed.</response>
        /// <response code="400">Failed to update password. Error list in response body.</response>
        /// <response code="404">The user with the received email was not found.</response>
        [AllowAnonymous]
        [HttpPut("resetPassword")]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResertUserPasswordViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ErrorsToList());

            var user = await _userService.Users.FirstOrDefaultAsync(u => u.Email == model.UserEmail);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userService.ResetPasswordAsync(user, model.Code, model.Password);

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
            var nirmalizedEmail = email.Trim().ToUpper();

            var user = await _userService.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == nirmalizedEmail);
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
            if (String.IsNullOrEmpty(userId) || String.IsNullOrEmpty(code))
            {
                return BadRequest();
            }

            var user = await _userService.Users.FirstAsync(u => u.Id == userId);
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
            var user = await GetCurrentUserAsync();
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

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userService.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
        }
    }
}
