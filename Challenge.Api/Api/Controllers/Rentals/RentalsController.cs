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
    public class RentalsController : ControllerBase
    {
        /// <summary>El Objeto de logica de negocios</summary>
        private readonly RentalBL _rentalBL;

        /// <summary>
        /// Initialize a new instance of <see cref="RentalsController"/> class.
        /// </summary>
        /// <param name="configuration">The asssettings.json configuration.</param>
        public RentalsController(IOptions<BackEndConfiguration> configuration)
        {
            _rentalBL = new RentalBL(configuration);
        }

        /// <summary>Creates a new row for a <see cref="Rental"/> object.</summary>
        /// <param name="instance">The object to insert.</param>
        /// <returns>Once the object was created returns the current object data.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Movies.EL.Model.Rental), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Post([FromBody] Movies.EL.Model.Rental instance)
        {
            var data = await _rentalBL.Post(instance);
            
            if (data.id > 0)
            {
                return Ok(data);
            }
            return BadRequest();
        }

        /// <summary>Update the <see cref="Rental"/> with the provided id.</summary>
        /// <param name="instance">The object to update.</param>
        /// <returns>The updated object.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Movies.EL.Model.Rental), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put([FromBody] Movies.EL.Model.Rental instance, [FromRoute] int id)
        {
            instance.id = id;
            var data = await _rentalBL.Put(instance);
            if (data.id == id)
            {
                return Ok(data);
            }
            return BadRequest();
        }

        /// <summary>Update the <see cref="Rental"/> with the provided id.</summary>
        /// <param name="instance">The object to update.</param>
        /// <returns>The updated object.</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(Movies.EL.Model.Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Patch([FromBody] Movies.EL.Model.Rental instance, [FromRoute] int id)
        {
            instance.id = id;
            var data = await _rentalBL.Patch(instance);
            if (data.id == id)
            {
                return Ok(data);
            }
            return BadRequest();
        }

        /// <summary>Deletes an object <see cref="Movies.EL.Model.Rental"/> having its id.</summary>
        /// <param name="id">El unique id.</param>
        /// <returns>The deleted object.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Movies.EL.Model.Rental), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var data = await _rentalBL.Delete(id);
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
        [ProducesResponseType(typeof(PaginatedList<Movies.EL.Model.Rental>), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Get([FromBody] Dictionary<string, object> Params)
        {
            var data = await _rentalBL.Get(Params);
            return Ok(data);
        }
    }
}
