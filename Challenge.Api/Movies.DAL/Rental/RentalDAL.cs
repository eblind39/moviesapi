using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Movies.DAL.Base;
using Movies.EL.Configurations;
using Movies.EL.Model;
using Movies.EL.Utils;
using System.Linq;
using System.Collections.Generic;

namespace Movies.DAL
{
    public class RentalDAL: BaseDAL
    {
        /// <summary>
        /// Initialize a new instance of <see cref="RentalDAL" />.
        /// </summary>
        /// <param name="configuration">The appsettings.json configuration.</param>
        public RentalDAL(IOptions<BackEndConfiguration> configuration) : base(configuration) { }

        /// <summary>Inserts a new object <see cref="Rental" />.</summary>
        /// <param name="objeto">El objeto.</param>
        /// <returns>Objeto insertado</returns>
        public async Task<Rental> Post(Rental objeto)
        {
            _context.Entry(objeto).State = EntityState.Added;
            await _context.SaveChangesAsync();

            return objeto;
        }

        /// <summary>
        /// Updates the object <see cref="Rental" />..
        /// </summary>
        /// <param name="instance">The object to update.</param>
        /// <returns>The updated object.</returns>
        public async Task<Rental> Put(Rental instance)
        {
            _context.Entry(instance).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return instance;
        }

        /// <summary>
        /// Updates the object <see cref="Rental" />..
        /// </summary>
        /// <param name="instance">The object to update.</param>
        /// <returns>The updated object.</returns>
        public async Task<Rental> Patch(Rental instance)
        {
            _context.Entry(instance).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return instance;
        }

        /// <summary>
        /// Get the object <see cref="Rental" /> given its id.
        /// </summary>
        /// <param name="id">The unique id.</param>
        /// <returns>
        /// Returns an object <see cref="Rental" /> if everything goes ok.
        /// </returns>
        public async Task<Rental> GetById(int id)
        {
            var instance = await _context.Rental
                .AsNoTracking()
                .Include(mv => mv.Movies)
                .FirstOrDefaultAsync(x => x.id == id);

            return instance;
        }

        /// <summary>
        /// Deletes a <see cref="Rental" /> object.
        /// </summary>
        /// <param name="instance">The object to delete.</param>
        /// <returns>True / false if the operation was ok / failed.</returns>
        public async Task<bool> Delete(Rental instance)
        {
            //_context.Entry(instance).State = EntityState.Deleted;
            //await _context.SaveChangesAsync();

            _context.Rental.Remove(instance);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>Returns a paginated list.</summary>
        /// <param name="unavailable">Filter by available field.</param>
        /// <param name="size">The size of the page requested.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="sort">Sorting criteria using the format: field_name[,asc|,desc].</param>
        /// <returns>
        /// A collection of <see cref="PaginatedList{T}" /> de tipo <see cref="Movies.EL.Model.Rental"
        /// </returns>
        public async Task<PaginatedList<Rental>> Get(
            string sort = null,
            int? size = 12,
            int? page = 1)
        {
            // TODO - 
            var query = _context.Rental
                .AsNoTracking()
                // .Include(mv => mv.movieIdNavigation)
                .Include(mv => mv.Movies)
                .Where(x => x.id > 0);

            #region filtros

            if (!string.IsNullOrEmpty(sort))
            {
                query = query.Where(x => x.customerEmail.Contains(sort));
            }

            #endregion

            query = query.OrderByDescending(x => x.movieId);

            var respuesta = await PaginatedList<Rental>.CreateAsync(query, page ?? 1, size ?? 12);

            return respuesta;
        }

        /// <summary>Returns the list of Rentals found between from and to.</summary>
        /// <param name="from">Start date.</param>
        /// <param name="to">End date</param>
        /// <returns>
        /// A List of <see cref="List<Rental>" /> de tipo <see cref="Movies.EL.Model.Rental"
        /// </returns>
        public async Task<List<Rental>> GetFromTo(int movieId, DateTime from, DateTime to)
        {
            // TODO - 
            var respuesta = await _context.Rental
                .AsNoTracking()
                .Where(x => (x.Created >= from && x.Created <= to) && (x.movieId == movieId)).ToListAsync();

            return respuesta;
        }
    }
}
