using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Movies.DAL.Base;
using Movies.EL.Configurations;
using Movies.EL.Model;
using Movies.EL.Utils;
using System.Linq;

namespace Movies.DAL
{
    public class MovieDAL: BaseDAL
    {
        /// <summary>
        /// Initialize a new instance of <see cref="MovieDAL" />.
        /// </summary>
        /// <param name="configuration">The appsettings.json configuration.</param>
        public MovieDAL(IOptions<BackEndConfiguration> configuration) : base(configuration) { }

        /// <summary>Inserta un nuevo objeto <see cref="Movie" />.</summary>
        /// <param name="objeto">El objeto.</param>
        /// <returns>Objeto insertado</returns>
        public async Task<Movie> Post(Movie objeto)
        {
            _context.Entry(objeto).State = EntityState.Added;
            await _context.SaveChangesAsync();

            return objeto;
        }

        /// <summary>
        /// Updates the object <see cref="Movie" />..
        /// </summary>
        /// <param name="instance">The object to update.</param>
        /// <returns>The updated object.</returns>
        public async Task<Movie> Put(Movie instance)
        {
            _context.Entry(instance).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return instance;
        }

        /// <summary>
        /// Updates the object <see cref="Movie" />..
        /// </summary>
        /// <param name="instance">The object to update.</param>
        /// <returns>The updated object.</returns>
        public async Task<Movie> Patch(Movie instance)
        {
            _context.Entry(instance).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return instance;
        }

        /// <summary>
        /// Get the object <see cref="Movie" /> given its id.
        /// </summary>
        /// <param name="movieId">The unique id.</param>
        /// <returns>
        /// Returns an object <see cref="Movie" /> if everything goes ok.
        /// </returns>
        public async Task<Movie> GetById(int movieId)
        {
            var instance = await _context.Movie
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.movieId == movieId);

            return instance;
        }

        /// <summary>
        /// Deletes a <see cref="Movie" /> object.
        /// </summary>
        /// <param name="instance">The object to delete.</param>
        /// <returns>True / false if the operation was ok / failed.</returns>
        public async Task<bool> Delete(Movie instance)
        {
            //_context.Entry(instance).State = EntityState.Deleted;
            //await _context.SaveChangesAsync();

            _context.Movie.Remove(instance);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>Returns a paginated list.</summary>
        /// <param name="unavailable">Filter by available field.</param>
        /// <param name="size">The size of the page requested.</param>
        /// <param name="page">The current page number.</param>
        /// <param name="sort">Sorting criteria using the format: field_name[,asc|,desc].</param>
        /// <returns>
        /// A collection of <see cref="PaginatedList{T}" /> de tipo <see cref="Movies.EL.Model.Movie"
        /// </returns>
        public async Task<PaginatedList<Movie>> Get(
            bool? unavailable = null,
            string sort = null,
            int? size = 12,
            int? page = 1)
        {
            // TODO - 
            var query = _context.Movie
                .AsNoTracking()
                .Where(x => x.available == true);

            #region filtros

            if (!string.IsNullOrEmpty(sort))
            {
                query = query.Where(x => x.title.Contains(sort));
            }

            #endregion

            query = query.OrderByDescending(x => x.movieId);

            var respuesta = await PaginatedList<Movie>.CreateAsync(query, page ?? 1, size ?? 12);

            return respuesta;
        }
    }
}
