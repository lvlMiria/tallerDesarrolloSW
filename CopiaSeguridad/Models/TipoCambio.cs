using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Models;

public partial class TipoCambio
{
    public byte Mes { get; set; }

    public short Anio { get; set; }

    [RegularExpression(@"^\d+(,\d{2})?$", ErrorMessage = "Utilice una coma como separador y solo dos decimales, por favor.")]
    public decimal Valor { get; set; }

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}
