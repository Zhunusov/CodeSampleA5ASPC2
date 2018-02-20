using System.Linq;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Identity;
using Servises.Interfaces.Base;

namespace Servises.Interfaces.AuthenticationServices
{
    /// <summary>
    /// Provides the APIs for managing roles in a persistence store.
    /// </summary>
    public interface IRoleService: IBaseGenericDataService<string, UserRole>
    {
        /// <summary>
        /// Get user role by name.
        /// </summary>
        /// <param name="role">User role name.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the Domain.Identity.UserRole of the operation or null.</returns>
        Task<UserRole> GetByNameOrDafault(string role);

        /// <summary>
        /// Creates the specified role in the persistence store.
        /// </summary>
        /// <param name="role">The role to create.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the Domain.Core.ServiceResult of the operation.</returns>
        Task<ServiceResult> CreateAsync(string role);

        /// <summary>
        /// Deletes the specified role.
        /// </summary>
        /// <param name="role">The role to delete.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the Domain.Core.ServiceResult of the operation.</returns>
        Task<ServiceResult> DeleteAsync(string role);

        /// <summary>
        /// Updates the specified role.
        /// </summary>
        /// <param name="role">The role to updated.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the Domain.Core.ServiceResult of the operation.</returns>
        Task<ServiceResult> UpdateAsync(string role);
    }
}
