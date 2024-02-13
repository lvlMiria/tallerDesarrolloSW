using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Models;

public partial class Ipc
{
    public short Anio { get; set; }
    [RegularExpression(@"^\d+(,\d{1})?$", ErrorMessage = "Utilice una coma como separador y solo un decimal, por favor.")]
    public decimal? Valor { get; set; }
}
