using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.GuidelinesControllers
{
    public interface IFullRestApiController<in TKey, in TPostEntity, in TPutEntity>
    {
        /// <summary>
        /// Get count of entities.
        /// </summary>
        Task<IActionResult> CountAsync();

        /// <summary>
        /// Get list of entities.
        /// </summary>
        /// <response code="200">Success.</response>
        /// <response code="400">Failed to get entities. Error list in response body.</response>
        [ProducesResponseType(typeof(List<string>), 400)]
        Task<IActionResult> GetAsync(int? count, int? offset = 0);

        /// <summary>
        /// Get entitiy by id.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        /// <response code="200">Success.</response>
        /// <response code="400">Failed to get entity. Error list in response body.</response>
        /// <response code="404">The entity with the received id was not found.</response>
        [ProducesResponseType(typeof(List<string>), 400)]
        Task<IActionResult> GetAsync(TKey id);

        /// <summary>
        /// Chang entity.
        /// </summary>
        /// <param name="putViewModel"></param>
        /// <response code="200">The entity was changed.</response>
        /// <response code="400">Failed to change entity. Error list in response body.</response>
        [ProducesResponseType(typeof(List<string>), 400)]
        Task<IActionResult> PutAsync([FromBody] TPutEntity putViewModel);

        /// <summary>
        /// Create entity.
        /// </summary>
        /// <param name="postViewModel"></param>
        /// <response code="201">The entity was created.</response>
        /// <response code="400">Failed to create entity. Error list in response body.</response>
        [ProducesResponseType(typeof(List<string>), 400)]
        Task<IActionResult> PostAsync([FromBody] TPostEntity postViewModel);

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="id">Id of entity to delete.</param>
        /// <response code="200">The entity was deleted.</response>
        /// <response code="400">Failed to delete entity. Error list in response body.</response>
        /// <response code="404">The entity with the received id was not found.</response>
        [ProducesResponseType(typeof(List<string>), 400)]
        Task<IActionResult> DeleteAsync(TKey id);
    }

    public interface IFullRestApiController<in TKey, in TPostPutEntity>
    {
        /// <summary>
        /// Get count of entities.
        /// </summary>
        Task<IActionResult> CountAsync();

        /// <summary>
        /// Get list of entities.
        /// </summary>
        /// <response code="200">Success.</response>
        /// <response code="400">Failed to get entities. Error list in response body.</response>
        [ProducesResponseType(typeof(List<string>), 400)]
        Task<IActionResult> GetAsync(int? count, int? offset = 0);

        /// <summary>
        /// Get entitiy by id.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        /// <response code="200">Success.</response>
        /// <response code="400">Failed to get entity. Error list in response body.</response>
        /// <response code="404">The entity with the received id was not found.</response>
        [ProducesResponseType(typeof(List<string>), 400)]
        Task<IActionResult> GetAsync(TKey id);

        /// <summary>
        /// Chang entity.
        /// </summary>
        /// <param name="putViewModel"></param>
        /// <response code="200">The entity was changed.</response>
        /// <response code="400">Failed to change entity. Error list in response body.</response>
        [ProducesResponseType(typeof(List<string>), 400)]
        Task<IActionResult> PutAsync([FromBody] TPostPutEntity putViewModel);

        /// <summary>
        /// Create entity.
        /// </summary>
        /// <param name="postViewModel"></param>
        /// <response code="201">The entity was created.</response>
        /// <response code="400">Failed to create entity. Error list in response body.</response>
        [ProducesResponseType(typeof(List<string>), 400)]
        Task<IActionResult> PostAsync([FromBody] TPostPutEntity postViewModel);

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="id">Id of entity to delete.</param>
        /// <response code="200">The entity was deleted.</response>
        /// <response code="400">Failed to delete entity. Error list in response body.</response>
        /// <response code="404">The entity with the received id was not found.</response>
        [ProducesResponseType(typeof(List<string>), 400)]
        Task<IActionResult> DeleteAsync(TKey id);
    }
}