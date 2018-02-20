using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Servises.Interfaces.AuthenticationServices;

namespace AuthenticationServices
{
    public sealed class RoleService : IRoleService
    {
        private readonly IQueryable<UserRole> _roles;

        private readonly RoleManager<UserRole> _roleManager;

        public RoleService(RoleManager<UserRole> roleManager)
        {
            _roleManager = roleManager;
            _roles = roleManager.Roles;
        }

        public Task<int> CountAsync()
        {
            return _roles.CountAsync();
        }

        public Task<UserRole> GetByIdOrDefaultAsync(string id)
        {
            return _roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task<UserRole> GetByNameOrDafault(string role)
        {
            var normalized = role.Trim().ToUpper();
            return _roles.FirstOrDefaultAsync(r => r.NormalizedName == normalized);
        }

        public Task<List<UserRole>> GetListAsync(int? count = null, int? offset = null)
        {
            var query = _roles.Skip(offset.GetValueOrDefault());

            if (count == null)
            {
                return query.ToListAsync();
            }

            return query.Take(count.Value).ToListAsync();
        }

        public Task<ServiceResult> CreateAsync(string role)
        {
            return CreateAsync(new UserRole(role));
        }

        public async Task<ServiceResult> CreateAsync(UserRole role)
        {
            return (await _roleManager.CreateAsync(role)).ToServiceResult();
        }

        public Task<ServiceResult> DeleteAsync(string role)
        {
            return DeleteAsync(new UserRole(role));
        }

        public async Task<ServiceResult> DeleteAsync(UserRole role)
        {
            return (await _roleManager.DeleteAsync(role)).ToServiceResult();
        }

        public Task<ServiceResult> UpdateAsync(string role)
        {
            return UpdateAsync(new UserRole(role));
        }

        public async Task<ServiceResult> UpdateAsync(UserRole role)
        {
            return (await _roleManager.UpdateAsync(role)).ToServiceResult();
        }
    }
}
