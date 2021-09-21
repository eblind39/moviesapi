using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Movies.EL.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Movies.EL.Model.Auxiliar;

namespace Movies.EL.Model
{
    public partial class Transaction
    {
        private static readonly char delimiter = ';';

        public Transaction()
        {
            this._rentals = this._customers = this._sales = "";
        }

        public int movieId { get; set; }

        public decimal totalRevenue { get; set; }

        public string _rentals { get; set; }
        [NotMapped]
        public string[] rentals 
        {
            get 
            {
                return _rentals.Split($"{delimiter}");
            }
            set
            {
                _rentals = string.Join($"{delimiter}", value);
            }
        }

        public string _sales { get; set; }
        [NotMapped]
        public string[] sales
        {
            get
            {
                return _sales.Split($"{delimiter}");
            }
            set
            {
                _sales = string.Join($"{delimiter}", value);
            }
        }

        public string _customers { get; set; }
        [NotMapped]
        public string[] customers
        {
            get
            {
                return _customers.Split($"{delimiter}");
            }
            set
            {
                _customers = string.Join($"{delimiter}", value);
            }
        }
    }
}
