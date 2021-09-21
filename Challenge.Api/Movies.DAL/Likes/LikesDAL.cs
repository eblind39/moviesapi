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
    public class LikesDAL: BaseDAL
    {
        /// <summary>
        /// Initialize a new instance of <see cref="LikesDAL" />.
        /// </summary>
        /// <param name="configuration">The appsettings.json configuration.</param>
        public LikesDAL(IOptions<BackEndConfiguration> configuration) : base(configuration) { }

        /// <summary>Inserts a new object <see cref="Likes" />.</summary>
        /// <param name="objeto">El objeto.</param>
        /// <returns>Objeto insertado</returns>
        public async Task<Likes> Post(Likes objeto)
        {
            _context.Entry(objeto).State = EntityState.Added;
            await _context.SaveChangesAsync();

            return objeto;
        }

        /// <summary>
        /// Updates the object <see cref="Likes" />..
        /// </summary>
        /// <param name="instance">The object to update.</param>
        /// <returns>The updated object.</returns>
        public async Task<Likes> Put(Likes instance)
        {
            _context.Entry(instance).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return instance;
        }

        /// <summary>
        /// Get the object <see cref="Likes" /> given its id.
        /// </summary>
        /// <param name="movieId">The unique movie id.</param>
        /// <returns>
        /// Returns an object <see cref="Likes" /> if everything goes ok.
        /// </returns>
        public async Task<Likes> GetByMovieId(int movieId)
        {
            var instance = await _context.Likes
                .AsNoTracking()
                // .Include(mv => mv.Movies)
                .FirstOrDefaultAsync(x => x.movieId == movieId);

            return instance;
        }

        /// <summary>
        /// Deletes a <see cref="Likes" /> object.
        /// </summary>
        /// <param name="instance">The object to delete.</param>
        /// <returns>True / false if the operation was ok / failed.</returns>
        public async Task<bool> Delete(Likes instance)
        {
            //_context.Entry(instance).State = EntityState.Deleted;
            //await _context.SaveChangesAsync();

            _context.Likes.Remove(instance);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>Returns a paginated list.</summary>
        /// <param name="unavailable">Filter by available field.</param>
        /// <param name="size">The size of the page requested.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="sort">Sorting criteria using the format: field_name[,asc|,desc].</param>
        /// <returns>
        /// A collection of <see cref="PaginatedList{T}" /> de tipo <see cref="Movies.EL.Model.Sale"
        /// </returns>
        public async Task<PaginatedList<Likes>> Get(
            string sort = null,
            int? size = 12,
            int? page = 1)
        {
            // TODO - 
            var query = _context.Likes
                .AsNoTracking()
                // .Include(mv => mv.movieIdNavigation)
                // .Include(mv => mv.Movies)
                .Where(x => x.movieId > 0);

            #region filtros

            if (!string.IsNullOrEmpty(sort))
            {
                query = query.Where(x => x.customers.Contains(sort));
            }

            #endregion

            query = query.OrderByDescending(x => x.movieId);

            var respuesta = await PaginatedList<Likes>.CreateAsync(query, page ?? 1, size ?? 12);

            return respuesta;
        }

        /// <summary>Returns the list of Likes found between from and to.</summary>
        /// <param name="from">Start date.</param>
        /// <param name="to">End date</param>
        /// <returns>
        /// A List of <see cref="List<Likes>" /> de tipo <see cref="Movies.EL.Model.Likes"
        /// </returns>
        public async Task<List<Likes>> GetFromTo(int movieId, DateTime from, DateTime to)
        {
            // TODO - 
            var respuesta = await _context.Likes
                .AsNoTracking()
                .Where(x => (x.Created >= from && x.Created <= to) && (x.movieId == movieId)).ToListAsync();

            return respuesta;
        }
    }
}
