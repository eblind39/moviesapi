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
    public class TransactionBL : BaseBL
    {
        /// <summary>
        /// Add reference to DAL operations
        /// </summary>
        private readonly RentalDAL _rentalDAL;
        private readonly SaleDAL _saleDAL;
        private readonly LikesDAL _likesDAL;

        /// <summary>
        /// Initialize a new instance of <see cref="TransactionBL" />.
        /// </summary>
        /// <param name="configuration">The appsettings.json configuration.</param>
        public TransactionBL(IOptions<BackEndConfiguration> configuration) : base(configuration)
        {
            _rentalDAL = new RentalDAL(configuration);
            _saleDAL = new SaleDAL(configuration);
            _likesDAL = new LikesDAL(configuration);
        }

        /// <summary>Returns a transaction object.</summary>
        /// <param name="from">Start date.</param>
        /// <param name="to">End date</param>
        /// <returns>
        /// A collection of <see cref="PaginatedList{T}" /> de tipo <see cref="Movies.EL.Model.Movie"
        /// </returns>
        public async Task<Transaction> Get(int movieId, DateTime from, DateTime to)
        {
            var rentals = await _rentalDAL.GetFromTo(movieId, from, to);
            var sales = await _saleDAL.GetFromTo(movieId, from, to);
            var likes = await _likesDAL.GetFromTo(movieId, from, to);

            Transaction transaction = new Transaction { movieId = movieId };

            foreach (var rental in rentals)
            {
                transaction.rentals = (String.Join(";", transaction.rentals) + ";" + rental.Created.ToString()).Split(';');
                transaction.totalRevenue += rental.price;
            }

            foreach (var sale in sales) { 
                transaction.sales = (String.Join(";", transaction.sales) + ";" + sale.Created.ToString()).Split(';');
                transaction.totalRevenue += sale.price;
            }

            foreach (var like in likes)
            {
                transaction.customers = (String.Join(";", transaction.customers) + ";" + like.Created.ToString()).Split(';');
            }

            return transaction;
        }
    }
}
