using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movies.EL;
using Movies.BL;
using Microsoft.Extensions.Options;
using Movies.EL.Configurations;
using Movies.EL.Model;
using Movies.EL.Utils;

namespace MoviesAPI.Controllers.Movie
{
    [Route("[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        /// <summary>El Objeto de logica de negocios</summary>
        private readonly MovieBL _movieBL;

        /// <summary>
        /// Initialize a new instance of <see cref="MoviesController"/> class.
        /// </summary>
        /// <param name="configuration">The asssettings.json configuration.</param>
        public MoviesController(IOptions<BackEndConfiguration> configuration)
        {
            _movieBL = new MovieBL(configuration);
        }

        /// <summary>Creates a new row for a <see cref="Movie"/> object.</summary>
        /// <param name="instance">The object to insert.</param>
        /// <returns>Once the object was created returns the current object data.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Movies.EL.Model.Movie), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Post([FromBody] Movies.EL.Model.Movie instance)
        {
            var data = await _movieBL.Post(instance);
            
            if (data.movieId > 0)
            {
                return new ObjectResult(data) { StatusCode = StatusCodes.Status201Created };
            }
            return BadRequest();
        }

        /// <summary>Update the <see cref="Movie"/> with the provided id.</summary>
        /// <param name="instance">The object to update.</param>
        /// <returns>The updated object.</returns>
        [HttpPut("{movieId}")]
        [ProducesResponseType(typeof(Movies.EL.Model.Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put([FromBody] Movies.EL.Model.Movie instance, [FromRoute] int movieId)
        {
            instance.movieId = movieId;
            var data = await _movieBL.Put(instance);
            if (data.movieId == movieId)
            {
                return Ok(data);
            }
            return BadRequest();
        }

        /// <summary>Update the <see cref="Movie"/> with the provided id.</summary>
        /// <param name="instance">The object to update.</param>
        /// <returns>The updated object.</returns>
        [HttpPatch("{movieId}")]
        [ProducesResponseType(typeof(Movies.EL.Model.Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Patch([FromBody] Movies.EL.Model.Movie instance, [FromRoute] int movieId)
        {
            instance.movieId = movieId;
            var data = await _movieBL.Patch(instance);
            if (data.movieId == movieId)
            {
                return Ok(data);
            }
            return BadRequest();
        }

        /// <summary>Deletes an object <see cref="Movies.EL.Model.Movie"/> having its id.</summary>
        /// <param name="id">El unique id.</param>
        /// <returns>The deleted object.</returns>
        [HttpDelete("{movieId}")]
        [ProducesResponseType(typeof(Movies.EL.Model.Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete([FromRoute] int movieId)
        {
            var data = await _movieBL.Delete(movieId);
            if (data == true)
            {
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>Returns a paginated list.</summary>
        /// <param name="Params">
        /// An object containing the needed parameters to get the pagination list.
        /// The list could be as:
        // Parameter	    type		values				        default
        // _______________________________________________________________________
        // unavailable      boolean     nullable                    null
        // size             int         nullable                    12
        // page             int         nullable                    1
        // sort             string      field_name[,asc|,desc]		title,desc
        /// </param>
        /// <returns>
        /// A collection of <see cref="PaginatedList{T}" /> de tipo <see cref="Movies.EL.Model.Movie"
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<Movies.EL.Model.Movie>), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Get([FromBody] Dictionary<string, object> Params)
        {
            var data = await _movieBL.Get(Params);
            return Ok(data);
        }
    }
}
