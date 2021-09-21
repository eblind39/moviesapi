using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Movies.EL.Configurations;
using Movies.EL.Model;
using Movies.BL.Base;
using Movies.DAL;
using System.Collections.Generic;
using Movies.EL.Utils;

namespace Movies.BL
{
    public class MovieBL : BaseBL
    {
        /// <summary>
        /// Add reference to DAL operations
        /// </summary>
        private readonly MovieDAL _movieDAL;

        /// <summary>
        /// Initialize a new instance of <see cref="MovieBL" />.
        /// </summary>
        /// <param name="configuration">The appsettings.json configuration.</param>
        public MovieBL(IOptions<BackEndConfiguration> configuration) : base(configuration)
        {
            _movieDAL = new MovieDAL(configuration);
        }

        /// <summary>
        /// Creates a new row for a <see cref="Movie"/> object.
        /// </summary>
        /// <param name="instance">The object to insert.</param>
        /// <returns>
        /// The inserted object of type <see cref="Movie" />
        /// </returns>
        public async Task<Movie> Post(Movie instance)
        {
            if (!BusinessValidations(instance)) return new Movie();

            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    // Insert
                    var resultado = await _movieDAL.Post(instance);

                    // Commit
                    transaccion.Commit();

                    // return
                    return resultado;
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Update the object <see cref="Movie" />.
        /// </summary>
        /// <param name="instance">The object to update.</param>
        /// <returns>The object <see cref="Movie" /> updated.</returns>
        public async Task<Movie> Put(Movie instance)
        {
            if (!BusinessValidations(instance)) return new Movie();

            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    // Update
                    var resultado = await _movieDAL.Put(instance);

                    // Commit
                    transaccion.Commit();

                    // Return
                    return resultado;
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Update the object <see cref="Movie" />.
        /// </summary>
        /// <param name="instance">The object to update.</param>
        /// <returns>The object <see cref="Movie" /> updated.</returns>
        public async Task<Movie> Patch(Movie instance)
        {
            if (!BusinessValidations(instance)) return new Movie();

            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    // Update
                    var resultado = await _movieDAL.Patch(instance);

                    // Commit
                    transaccion.Commit();

                    // Return
                    return resultado;
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>Deletes an object <see cref="Movie" />.</summary>
        /// <param name="movieId">The unique id.</param>
        /// <returns>The deleted <see cref="Movie" /> object.</returns>
        public async Task<bool> Delete(int movieId)
        {
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    // Get the object
                    var instance = await _movieDAL.GetById(movieId);

                    // Delete it
                    var resultado = await _movieDAL.Delete(instance);

                    // Commit
                    transaccion.Commit();

                    // Return
                    return true;
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    throw ex;
                }
            }
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
        public async Task<PaginatedList<Movie>> Get(Dictionary<string, object> Params)
        {
            #region Filtros

            int? size = null;
            if (Params["size"] != null)
            {
                size = Convert.ToInt32(Params["size"].ToString());
            }

            int? page = null;
            if (Params["page"] != null)
            {
                page = Convert.ToInt32(Params["page"].ToString());
            }

            string sort = null;
            if (Params["sort"] != null)
            {
                sort = Convert.ToString(Params["sort"]);
            }

            bool? unavailable = null;
            if (Params["unavailable"] != null)
            {
                unavailable = Convert.ToBoolean(Params["unavailable"]);
            }

            #endregion

            var response = await _movieDAL.Get(
                unavailable,
                sort,
                size,
                page);

            response.Params = Params;

            return response;
        }

        /// <summary>Additional business validations.</summary>
        /// <param name="instance">The object to validate.</param>
        /// <returns>Si es valido o no</returns>
        private bool BusinessValidations(Movie instance)
        {
            bool valido = false;

            if (instance.movieId > 0)
            {
                valido = true;
            }

            if (!string.IsNullOrEmpty(instance.title))
            {
                valido = true;
            }

            if (instance.stock >= 0)
            {
                valido = true;
            }

            if (instance.rentalPrice >= 0)
            {
                valido = true;
            }

            if (instance.salePrice >= 0)
            {
                valido = true;
            }

            return valido;
        }
    }
}
