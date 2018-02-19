using System.Linq;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Identity;

namespace Servises.Interfaces.AuthenticationServices
{
    /// <summary>
    /// Provides the APIs for managing roles in a persistence store.
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// Gets an IQueryable of Roles.
        /// </summary>
        IQueryable<UserRole> Roles { get; }

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
