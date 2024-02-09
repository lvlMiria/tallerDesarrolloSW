using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Models;

public partial class TipoCambio
{
    [Range(1, 12, ErrorMessage = "Los meses deben ser del {1} al {2}.")]
    public byte Mes { get; set; }

    public short Anio { get; set; }

    public int? Valor { get; set; }

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}
