using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.EL.DataAnnotations
{
    class ValidationMessages
    {
        #region Common
        /// <summary>
        /// The prompt/placeholder message
        /// </summary>
        public const string Prompt = "Please enter {0}.";

        /// <summary>
        /// The required error
        /// </summary>
        public const string Required_Error = "The field {0} is required.";
        #endregion

        #region Strings
        /// <summary>
        /// The string max length error
        /// </summary>
        public const string String_Max_Error = "The required length for {0} is {1:n0} characters.";
        #endregion

        #region Int
        public const string Int_Range_Value = "The field {0} must be between {1:n0} and {2:n0}.";
        #endregion

        #region Decimal
        public const string Decimal_Range_Value = "The field {0} must be between {1:n0} and {2:n0}.";
        #endregion

        #region CSharpMaxAndMinValues
        public const string Csharp_Int_MaxValue = "+2147483647";

        public const string Csharp_Decimal_MaxValue = "+79228162514264337593543950335";
        #endregion
    }
}
