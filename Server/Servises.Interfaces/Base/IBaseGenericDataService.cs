using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Core.Base;

namespace Servises.Interfaces.Base
{
    public interface IBaseGenericDataService<TKey, TEntity> where TEntity : IBaseEntity<TKey>
    {
        /// <summary>
        /// Get count of entities.
        /// </summary>
        /// <returns>Count of entities</returns>
        Task<int> CountAsync();

        /// <summary>
        /// Get entity by primary key.
        /// </summary>
        /// <param name="id">Primary key of entity.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the TEntity of the operation or null.</returns>
        Task<TEntity> GetByIdOrDefaultAsync(TKey id);

        /// <summary>
        /// Get list of entities.
        /// </summary>
        /// <param name="count">Count of entities to load.</param>
        /// <param name="offset">Offset.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the List TEntity of the operation.</returns>
        Task<List<TEntity>> GetListAsync(int? count = null, int? offset = null);

        /// <summary>
        /// Create new entity.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the Domain.Core.ServiceResult of the operation.</returns>
        Task<ServiceResult> CreateAsync(TEntity entity);

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the Domain.Core.ServiceResult of the operation.</returns>
        Task<ServiceResult> DeleteAsync(TEntity entity);

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the Domain.Core.ServiceResult of the operation.</returns>
        Task<ServiceResult> UpdateAsync(TEntity entity);
    }
}