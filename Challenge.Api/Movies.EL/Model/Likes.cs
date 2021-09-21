using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Movies.EL.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Movies.EL.Model.Auxiliar;

namespace Movies.EL.Model
{
    public partial class Likes : IAuditable
    {
        private static readonly char delimiter = ';';

        public Likes()
        {
            // Movies = new HashSet<Movie>();
        }

        [Required(ErrorMessage = ValidationMessages.Required_Error), DataType(DataType.Text)]
        [Display(Name = "Movie")]
        [Range(type: typeof(int), minimum: "0", maximum: ValidationMessages.Csharp_Int_MaxValue, ErrorMessage = ValidationMessages.Int_Range_Value)]
        public int movieId { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Error), DataType(DataType.Text)]
        [Display(Name = "Likes")]
        [Range(type: typeof(int), minimum: "0", maximum: ValidationMessages.Csharp_Int_MaxValue, ErrorMessage = ValidationMessages.Int_Range_Value)]
        public int likes { get; set; }

        public string _customers { get; set; }
        [NotMapped]
        public string[] customers {
            get 
            {
                return _customers.Split($"{delimiter}");
            }
            set
            {
                _customers = string.Join($"{delimiter}", value);
            }
        }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        // public virtual IEnumerable<Movie> Movies { get; set; }
    }
}
