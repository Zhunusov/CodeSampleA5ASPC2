using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationServices;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Servises.Interfaces.AuthenticationServices;

namespace Web.Initializers
{
    public static class InitializeService
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleService = services.GetRequiredService<IRoleService>();
            var userService = services.GetRequiredService<IUserService>();
            var json = File.ReadAllText("admin.json");
            var adminModel = JsonConvert.DeserializeObject<AdminModel>(json);
            await RoleInitializeAsync(roleService, Roles.Admin);
            await AdminInitializeAsync(userService, adminModel);
        }

        private static async Task RoleInitializeAsync(IRoleService roleService, string roleName)
        {
            var role = await roleService.GetByNameOrDafault(roleName);
            if (role == null)
            {
                var result = await roleService.CreateAsync(roleName);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First());
                }
            }
        }

        private static async Task AdminInitializeAsync(IUserService userService, AdminModel adminModel)
        {
            var defAdmin = await userService.GetByUserNameOrDefaultAsync(adminModel.UserName);
            if (defAdmin == null)
            {
                defAdmin = new ApplicationUser
                {
                    UserName = adminModel.UserName,
                    Email = adminModel.Email
                };

                var result = await userService.CreateAsync(defAdmin, adminModel.Password);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First());
                }

                result = await userService.AddToRoleAsync(defAdmin, "ADMIN");
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First());
                }
            }
        }
    }
}
