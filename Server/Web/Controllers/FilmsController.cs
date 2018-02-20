using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationServices;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servises.Interfaces;
using Web.Extensions;
using Web.GuidelinesControllers;

namespace Web.Controllers
{
    /// <summary>
    /// Films API.
    /// </summary>
    [Authorize(Roles = Roles.Admin)]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public sealed class FilmsController : Controller, IFullRestApiController<long, Film>
    {
        private readonly IFilmsService _dataService;

        public FilmsController(IFilmsService filmsService)
        {
            _dataService = filmsService;
        }

        /// <summary>
        /// Search films.
        /// </summary>
        /// <response code="200">Success.</response>
        /// <response code="400">Failed to get films. Error list in response body.</response>
        [AllowAnonymous]
        [HttpGet("search")]
        [ProducesResponseType(typeof(List<Film>), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> SearchAsync(int? count, int? offset, string searchString)
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

            if (string.IsNullOrEmpty(searchString))
            {
                errors.Add("The search string can not be empty.");
            }

            if (errors.Any())
            {
                return BadRequest(errors);
            }

            if (count == null)
            {
                return Ok(await _dataService.Entities
                    .Where(f => f.Name.Contains(searchString) || f.Description.Contains(searchString) || f.Director.Contains(searchString))
                    .Skip(offset.GetValueOrDefault()).ToListAsync());
            }
            return Ok(await _dataService.Entities
                .Where(f => f.Name.Contains(searchString) || f.Description.Contains(searchString) || f.Director.Contains(searchString))
                .Skip(offset.GetValueOrDefault()).Take(count.Value).ToListAsync());
        }

        /// <summary>
        /// Gel count of films.
        /// </summary>
        /// <response code="200">Success.</response>
        [AllowAnonymous]
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> CountAsync()
        {
            return Ok(await _dataService.Entities.CountAsync());
        }

        /// <summary>
        /// Gel list of films.
        /// </summary>
        /// <response code="200">Success.</response>
        /// <response code="400">Failed to get films. Error list in response body.</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(List<Film>), 200)]
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

            if (count == null)
            {
                return Ok(await _dataService.Entities.Skip(offset.GetValueOrDefault()).ToListAsync());
            }
            return Ok(await _dataService.Entities.Skip(offset.GetValueOrDefault()).Take(count.Value).ToListAsync());
        }

        /// <summary>
        /// Get film by id.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        /// <response code="200">Success.</response>
        /// <response code="400">Failed to get film. Error list in response body.</response>
        /// <response code="404">The film with the received id was not found.</response>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Film), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> GetAsync(long id)
        {
            if (id < 1)
            {
                return BadRequest(new List<string> {"Id can not be less than 1."});
            }

            var entity = await _dataService.Entities.FirstOrDefaultAsync(u => u.Id == id);

            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        /// <summary>
        /// Chang film.
        /// </summary>
        /// <param name="putViewModel"></param>
        /// <response code="200">The film was changed.</response>
        /// <response code="400">Failed to change film. Error list in response body.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">You are not the administrator.</response>
        [HttpPut]
        [ProducesResponseType(typeof(Film), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> PutAsync([FromBody] Film putViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ErrorsToList());

            var result = await _dataService.UpdateAsync(putViewModel);

            if (result.Succeeded)
            {
                return Ok(putViewModel);
            }

            return result.ToActionResult();
        }

        /// <summary>
        /// Create new film.
        /// </summary>
        /// <param name="postViewModel"></param>
        /// <response code="201">The film was created.</response>
        /// <response code="400">Failed to create film. Error list in response body.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">You are not the administrator.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Film), 201)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> PostAsync([FromBody] Film postViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ErrorsToList());

            var result = await _dataService.CreateAsync(postViewModel);

            if (result.Succeeded)
            {
                return new CreatedResult(Request.GetDisplayUrl() + $"/{postViewModel.Id}", postViewModel);
            }

            return result.ToActionResult();
        }

        /// <summary>
        /// Delete film.
        /// </summary>
        /// <param name="id">Id of film to delete.</param>
        /// <response code="200">The film was deleted.</response>
        /// <response code="400">Failed to delete film. Error list in response body.</response>
        /// <response code="401">User is not authorized.</response>
        /// <response code="403">You are not the administrator.</response>
        /// <response code="404">The film with the received id was not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            if (id < 1)
            {
                return BadRequest(new List<string> { "Id can not be less than 1"});
            }

            var entity = await _dataService.Entities.FirstOrDefaultAsync(u => u.Id == id);

            if (entity == null)
            {
                return NotFound();
            }

            var result = await _dataService.DeleteAsync(entity);

            return result.ToActionResult();
        }
    }
}
