using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Servises.Interfaces.AuthenticationServices;
using Servises.Interfaces.Base;

namespace AuthenticationServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IQueryable<ApplicationUser> _users;

        public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _users = userManager.Users;
        }

        public Task<int> CountAsync()
        {
            return _users.CountAsync();
        }

        public Task<ApplicationUser> GetByUserNameOrEmailOrDefaultAsync(string nameOrEmail)
        {
            var mormalized = nameOrEmail.Trim().ToUpper();

            return _users.FirstOrDefaultAsync(
                u => u.NormalizedUserName == mormalized ||
                     u.NormalizedEmail == mormalized);
        }

        public Task<ApplicationUser> GetByUserNameOrDefaultAsync(string userName)
        {
            var normalizedUserName = userName.Trim().ToUpper();
            return _users.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName);
        }

        public Task<ApplicationUser> GetByEmailOrDefaultAsync(string email)
        {
            var normalizedEmail = email.Trim().ToUpper();
            return _users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
        }

        public Task<ApplicationUser> GetByIdOrDefaultAsync(string id)
        {
            return _users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public Task<List<ApplicationUser>> GetListAsync(int? count = null, int? offset = null)
        {
            var query = _users.Skip(offset.GetValueOrDefault());
            if (count == null)
            {
                return query.ToListAsync();
            }

            return _users.Skip(offset.GetValueOrDefault()).Take(count.Value).ToListAsync();
        }

        public async Task<ServiceResult> CreateAsync(ApplicationUser user)
        {
            return (await _userManager.CreateAsync(user)).ToServiceResult();
        }

        public async Task<ServiceResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return (await _userManager.AddToRoleAsync(user, role)).ToServiceResult();
        }

        public async Task<ServiceResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            return (await _userManager.ChangePasswordAsync(user, currentPassword, newPassword)).ToServiceResult();
        }

        public async Task<ServiceResult> ConfirmEmailAsync(ApplicationUser user, string code)
        {
            return (await _userManager.ConfirmEmailAsync(user, code)).ToServiceResult();
        }

        public async Task<ServiceResult> CreateAsync(ApplicationUser user, string password)
        {
            return (await _userManager.CreateAsync(user, password)).ToServiceResult();
        }

        public async Task<ServiceResult> DeleteAsync(ApplicationUser user)
        {
            return (await _userManager.DeleteAsync(user)).ToServiceResult();
        }

        public Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            return _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            return _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<List<string>> GetRolesAsync(ApplicationUser user)
        {
            return (await _userManager.GetRolesAsync(user)).ToList();
        }

        public Task<ServiceResult> LockOutAsync(ApplicationUser user, DateTime date)
        {
            user.LockoutEnd = date;
            user.LockoutEnabled = true;
            return UpdateAsync(user);
        }

        public async Task<ServiceResult> RemoveFromRoleAsync(ApplicationUser user, string role)
        {
            return (await _userManager.RemoveFromRoleAsync(user, role)).ToServiceResult();
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string role)
        {
            return _userManager.IsInRoleAsync(user, role);
        }

        public async Task<ServiceResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            return (await _userManager.ResetPasswordAsync(user, token, newPassword)).ToServiceResult();
        }

        public async Task<ServiceResult> UpdateAsync(ApplicationUser user)
        {
            return (await _userManager.UpdateAsync(user)).ToServiceResult();
        }

        public Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return _userManager.CheckPasswordAsync(user, password);
        }

        public Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _users.FirstOrDefaultAsync(u => u.NormalizedUserName == "ADMIN");
            return _users.FirstOrDefaultAsync(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);
        }
    }
}
