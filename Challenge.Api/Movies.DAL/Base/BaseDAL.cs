using Movies.DAL.Model;
using Microsoft.Extensions.Options;
using Movies.EL.Configurations;

namespace Movies.DAL.Base
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseDAL
    {
        /// <summary>The instance to use the DBContext.</summary>
        protected readonly MovieDBContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDAL"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public BaseDAL(IOptions<BackEndConfiguration> configuration)
        {
            _context = new MovieDBContext(configuration);
        }


    }
}
