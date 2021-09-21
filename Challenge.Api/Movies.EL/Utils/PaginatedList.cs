using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.EL.Utils
{
    public interface IPaginatedList
    {
        int number { get; set; }
        int size { get; set; }
        int totalPages { get; set; }
        int totalElements { get; set; }
        string searchString { get; set; }
        Dictionary<string, object> Params { get; set; }
    }

    public interface IPaginatedList<T> : IPaginatedList, IEnumerable<T>
    {
    }

    [JsonObject]
    public class PaginatedList<T> : List<T>, IPaginatedList<T>
    {
        public int number { get; set; }
        public int size { get; set; }
        public int totalPages { get; set; }
        public int totalElements { get; set; }
        public string searchString { get; set; }
        public Dictionary<string, object> Params { get; set; }

        public PaginatedList()
        {
            Params = new Dictionary<string, object>();
        }

        private PaginatedList(List<T> source, int totalElements, int numPage, int size)
        {
            this.number = numPage;
            this.size = size;
            this.totalElements = totalElements;
            this.totalPages = (int)Math.Ceiling(this.totalElements / (double)size);

            this.AddRange(source);
        }

        public BindingList<T> Data
        {
            get
            {
                if (_dataTransfer == null)
                {
                    _dataTransfer = new BindingList<T>(this);
                    _dataTransfer.AddingNew += _dataTransfer_AddingNew;
                }
                return _dataTransfer;
            }
        }

        private void _dataTransfer_AddingNew(object sender, AddingNewEventArgs e)
        {
            this.Add((T)e.NewObject);
        }

        private BindingList<T> _dataTransfer;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int numPagina, int porPagina)
        {
            var totalElementos = await source.CountAsync();
            var items = await source.Skip((numPagina - 1) * porPagina).Take(porPagina).ToListAsync();
            return new PaginatedList<T>(items, totalElementos, numPagina, porPagina);
        }

    }
}