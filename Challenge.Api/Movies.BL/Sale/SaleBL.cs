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
    public class SaleBL : BaseBL
    {
        /// <summary>
        /// Add reference to DAL operations
        /// </summary>
        private readonly SaleDAL _saleDAL;

        /// <summary>
        /// Initialize a new instance of <see cref="SaleBL" />.
        /// </summary>
        /// <param name="configuration">The appsettings.json configuration.</param>
        public SaleBL(IOptions<BackEndConfiguration> configuration) : base(configuration)
        {
            _saleDAL = new SaleDAL(configuration);
        }

        /// <summary>
        /// Creates a new row for a <see cref="Sale"/> object.
        /// </summary>
        /// <param name="instance">The object to insert.</param>
        /// <returns>
        /// The inserted object of type <see cref="Sale" />
        /// </returns>
        public async Task<Sale> Post(Sale instance)
        {
            if (!BusinessValidations(instance)) return new Sale();

            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    // Insert
                    var resultado = await _saleDAL.Post(instance);

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
        /// Update the object <see cref="Sale" />.
        /// </summary>
        /// <param name="instance">The object to update.</param>
        /// <returns>The object <see cref="Sale" /> updated.</returns>
        public async Task<Sale> Put(Sale instance)
        {
            if (!BusinessValidations(instance)) return new Sale();

            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    // Update
                    var resultado = await _saleDAL.Put(instance);

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
        /// Update the object <see cref="Sale" />.
        /// </summary>
        /// <param name="instance">The object to update.</param>
        /// <returns>The object <see cref="Sale" /> updated.</returns>
        public async Task<Sale> Patch(Sale instance)
        {
            if (!BusinessValidations(instance)) return new Sale();

            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    // Update
                    var resultado = await _saleDAL.Patch(instance);

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

        /// <summary>Deletes an object <see cref="Sale" />.</summary>
        /// <param name="movieId">The unique id.</param>
        /// <returns>The deleted <see cref="Sale" /> object.</returns>
        public async Task<bool> Delete(int id)
        {
            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    // Get the object
                    var instance = await _saleDAL.GetById(id);

                    // Delete it
                    var resultado = await _saleDAL.Delete(instance);

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
        public async Task<PaginatedList<Sale>> Get(Dictionary<string, object> Params)
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

            var response = await _saleDAL.Get(
                sort,
                size,
                page);

            response.Params = Params;

            return response;
        }

        /// <summary>Additional business validations.</summary>
        /// <param name="instance">The object to validate.</param>
        /// <returns>Si es valido o no</returns>
        private bool BusinessValidations(Sale instance)
        {
            bool valido = false;

            if (instance.id > 0)
            {
                valido = true;
            }

            if (!string.IsNullOrEmpty(instance.customerEmail))
            {
                valido = true;
            }

            if (instance.price >= 0)
            {
                valido = true;
            }

            return valido;
        }
    }
}
