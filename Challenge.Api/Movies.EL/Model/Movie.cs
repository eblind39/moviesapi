using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Movies.EL.DataAnnotations;
using Movies.EL.Model.Auxiliar;

namespace Movies.EL.Model
{
    public partial class Movie : IAuditable
    {

        public int movieId { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Error), DataType(DataType.Text)]
        [Display(Name = "Title")]
        [MaxLength(100, ErrorMessage = ValidationMessages.String_Max_Error)]
        public string title { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        [MaxLength(255, ErrorMessage = ValidationMessages.String_Max_Error)]
        public string description { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Error), DataType(DataType.Text)]
        [Display(Name = "Stock")]
        [Range(type: typeof(int), minimum: "0", maximum: ValidationMessages.Csharp_Int_MaxValue, ErrorMessage = ValidationMessages.Int_Range_Value)]
        public int stock { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Error), DataType(DataType.Text)]
        [Display(Name = "Rental price")]
        [Range(type: typeof(decimal), minimum: "0", maximum: ValidationMessages.Csharp_Decimal_MaxValue, ErrorMessage = ValidationMessages.Decimal_Range_Value)]
        public decimal? rentalPrice { get; set; }

        [Required(ErrorMessage = ValidationMessages.Required_Error), DataType(DataType.Text)]
        [Display(Name = "Sale price")]
        [Range(type: typeof(decimal), minimum: "0", maximum: ValidationMessages.Csharp_Decimal_MaxValue, ErrorMessage = ValidationMessages.Decimal_Range_Value)]
        public decimal? salePrice { get; set; }

        [Display(Name = "Is it available?")]
        public bool? available { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        // public virtual Movie movieIdNavigation { get; set; }
        public Sale Sale { get; set; }
        public Rental Rental { get; set; }
        public Likes Likes { get; set; }
    }
}
