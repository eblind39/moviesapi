using System;
using System.Collections.Generic;
using System.Text;

namespace Movies.EL.Model.Auxiliar
{
    public interface IAuditable
    {
        DateTime Created { get; set; }
        DateTime Modified { get; set; }
    }
}
