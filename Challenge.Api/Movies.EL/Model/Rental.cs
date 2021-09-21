using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Movies.EL.DataAnnotations;
using Movies.EL.Model.Auxiliar;

namespace Movies.EL.Model
{
    public partial class Rental : IAuditable
    {

        public Rental()
        {
            Movies = new HashSet<Movie>();
        }

        public int id { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Error), DataType(DataType.Text)]
        [Display(Name = "Movie")]
        [Range(type: typeof(int), minimum: "0", maximum: ValidationMessages.Csharp_Int_MaxValue, ErrorMessage = ValidationMessages.Int_Range_Value)]
        public int movieId { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Error), DataType(DataType.Text)]
        [Display(Name = "Customer email")]
        [MaxLength(100, ErrorMessage = ValidationMessages.String_Max_Error)]
        public string customerEmail { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Error), DataType(DataType.Text)]
        [Display(Name = "Price")]
        [Range(type: typeof(decimal), minimum: "0", maximum: ValidationMessages.Csharp_Decimal_MaxValue, ErrorMessage = ValidationMessages.Decimal_Range_Value)]
        public decimal price { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public virtual IEnumerable<Movie> Movies { get; set; }
    }
}
