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
using Movies.EL.Model.Auxiliar;
using Movies.EL.Utils;

namespace MoviesAPI.Controllers.Movie
{
    [Route("[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        /// <summary>El Objeto de logica de negocios</summary>
        private readonly LikesBL _likesBL;

        /// <summary>
        /// Initialize a new instance of <see cref="LikesController"/> class.
        /// </summary>
        /// <param name="configuration">The asssettings.json configuration.</param>
        public LikesController(IOptions<BackEndConfiguration> configuration)
        {
            _likesBL = new LikesBL(configuration);
        }

        /// <summary>Creates a new row for a <see cref="Likes"/> object.</summary>
        /// <param name="movieId">The unique movid id.</param>
        /// <param name="customerEmail">The customer email.</param>
        /// <returns>Once the object was created returns the current object data.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Movies.EL.Model.Likes), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Post([FromBody] LikesParams likesParams)
        {
            var data = await _likesBL.Post(likesParams.movieId, likesParams.customerEmail);
            
            if (data.movieId > 0)
            {
                return Ok(data);
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
        [ProducesResponseType(typeof(PaginatedList<Movies.EL.Model.Likes>), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Get([FromBody] Dictionary<string, object> Params)
        {
            var data = await _likesBL.Get(Params);
            return Ok(data);
        }
    }
}
