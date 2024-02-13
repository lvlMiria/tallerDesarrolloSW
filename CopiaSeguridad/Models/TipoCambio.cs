using System;
using System.Collections.Generic;

namespace Presentacion.Models;

public partial class TipoCambio
{
    public byte Mes { get; set; }

    public short Anio { get; set; }

    public int? Valor { get; set; }

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}
