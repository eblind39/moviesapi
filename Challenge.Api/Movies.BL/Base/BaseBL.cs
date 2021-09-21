using Movies.DAL.Model;
using Movies.EL.Configurations;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Movies.BL.Base
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseBL
    {
        /// <summary>The context</summary>
        protected readonly MovieDBContext _context;

        /// <summary>Initializes a new instance of the <see cref="BaseBL"/> class.</summary>
        /// <param name="configuration">The configuration.</param>
        public BaseBL(IOptions<BackEndConfiguration> configuration)
        {
            _context = new MovieDBContext(configuration);
        }
    }
}
