using System.Linq;
using System.Threading.Tasks;
using Domain.Core;

namespace Servises.Interfaces.BaseServices
{
    /// <summary>
    /// Provides the APIs for managing entity in a persistence store.
    /// </summary>
    public interface IGenericDataService<TEntity> where TEntity : class
    {
        /// <summary>
        /// Returns an IQueryable of entities.
        /// </summary>
        IQueryable<TEntity> Entities { get; }

        /// <summary>
        /// Creates the specified entity in the backing store.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the Domain.Core.ServiceResult of the operation.</returns>
        Task<ServiceResult> CreateAsync(TEntity entity);

        /// <summary>
        /// Deletes the specified entity from the backing store.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the Domain.Core.ServiceResult of the operation.</returns>
        Task<ServiceResult> DeleteAsync(TEntity entity);

        /// <summary>
        /// Updates the specified entity in the backing store.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the Domain.Core.ServiceResult of the operation.</returns>
        Task<ServiceResult> UpdateAsync(TEntity entity);
    }
}