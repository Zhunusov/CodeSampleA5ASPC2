using System.Linq;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Servises.Interfaces.AuthenticationServices;

namespace AuthenticationServices
{
    public class RoleService : IRoleService
    {
        public IQueryable<UserRole> Roles { get; }

        private readonly RoleManager<UserRole> _roleManager;

        public RoleService(RoleManager<UserRole> roleManager)
        {
            _roleManager = roleManager;
            Roles = roleManager.Roles;
        }

        public async Task<ServiceResult> CreateAsync(string role)
        {
            return (await _roleManager.CreateAsync(new UserRole(role))).ToServiceResult();
        }

        public async Task<ServiceResult> DeleteAsync(string role)
        {
            return (await _roleManager.DeleteAsync(new UserRole(role))).ToServiceResult();
        }

        public async Task<ServiceResult> UpdateAsync(string role)
        {
            return (await _roleManager.UpdateAsync(new UserRole(role))).ToServiceResult();
        }
    }
}
