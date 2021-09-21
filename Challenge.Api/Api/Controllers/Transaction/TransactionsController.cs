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
    public class TransactionsController : ControllerBase
    {
        /// <summary>El Objeto de logica de negocios</summary>
        private readonly TransactionBL _transactionBL;

        /// <summary>
        /// Initialize a new instance of <see cref="TransactionsController"/> class.
        /// </summary>
        /// <param name="configuration">The asssettings.json configuration.</param>
        public TransactionsController(IOptions<BackEndConfiguration> configuration)
        {
            _transactionBL = new TransactionBL(configuration);
        }

        /// <summary>Returns a transaction object.</summary>
        /// <param name="from">Start date.</param>
        /// <param name="to">End date</param>
        /// <returns>
        /// A collection of <see cref="PaginatedList{T}" /> de tipo <see cref="Movies.EL.Model.Movie"
        /// </returns>
        [HttpGet("movies/{movieId}")]
        [ProducesResponseType(typeof(Movies.EL.Model.Transaction), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Get([FromRoute] int movieId, DateTime from, DateTime to)
        {
            var data = await _transactionBL.Get(movieId, from, to);
            return Ok(data);
        }
    }
}
