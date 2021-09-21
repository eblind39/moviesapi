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
    public class LikesBL : BaseBL
    {
        /// <summary>
        /// Add reference to DAL operations
        /// </summary>
        private readonly LikesDAL _likesDAL;

        /// <summary>
        /// Initialize a new instance of <see cref="LikesBL" />.
        /// </summary>
        /// <param name="configuration">The appsettings.json configuration.</param>
        public LikesBL(IOptions<BackEndConfiguration> configuration) : base(configuration)
        {
            _likesDAL = new LikesDAL(configuration);
        }

        /// <summary>
        /// Creates a new row for a <see cref="Likes"/> object.
        /// </summary>
        /// <param name="movieId">The unique movid id.</param>
        /// <param name="customerEmail">The customer email.</param>
        /// <returns>
        /// The inserted object of type <see cref="Likes" />
        /// </returns>
        public async Task<Likes> Post(int movieId, string customerEmail)
        {
            Likes instance = new Likes { movieId = movieId, customers = new string[] { customerEmail }, likes = 0 };
            if (!BusinessValidations(instance)) return new Likes();

            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    var resultado = new Likes();

                    // Check if there is already a Like stored for this movie
                    var likeInstance = await _likesDAL.GetByMovieId(instance.movieId);
                    if (likeInstance != null)
                    {
                        // update stock
                        likeInstance.likes++;
                        if (!likeInstance.customers.ToString().Contains(customerEmail)) likeInstance.customers = (String.Join(";", likeInstance.customers) + ";" + customerEmail).Split(';');
                        resultado = await _likesDAL.Put(likeInstance);
                    }
                    else
                    {
                        // Insert
                        instance.likes++;
                        resultado = await _likesDAL.Post(instance);
                    }

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

        /// <summary>Deletes an object <see cref="Likes" />.</summary>
        /// <param name="movieId">The unique movie id.</param>
        /// <returns>The deleted <see cref="Likes" /> object.</returns>
        public async Task<bool> Delete(int movieId)
        {
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    // Get the object
                    var instance = await _likesDAL.GetByMovieId(movieId);

                    // Delete it
                    var resultado = await _likesDAL.Delete(instance);

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
        public async Task<PaginatedList<Likes>> Get(Dictionary<string, object> Params)
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

            #endregion

            var response = await _likesDAL.Get(
                sort,
                size,
                page);

            response.Params = Params;

            return response;
        }

        /// <summary>Additional business validations.</summary>
        /// <param name="instance">The object to validate.</param>
        /// <returns>Si es valido o no</returns>
        private bool BusinessValidations(Likes instance)
        {
            bool valido = false;

            if (instance.movieId > 0)
            {
                valido = true;
            }

            if (instance.customers.Length > 0)
            {
                valido = true;
            }

            return valido;
        }
    }
}
