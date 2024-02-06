using System;
using System.Collections.Generic;

namespace Presentacion.Models;

public partial class Factura
{
    public string CodFactura { get; set; } = null!;

    public string? NumFactura { get; set; }

    public int? Monto { get; set; }

    public byte? MesContable { get; set; }

    public short? AnioContable { get; set; }

    public string? Empresa { get; set; }

    public virtual ICollection<ControlFactura> ControlFacturas { get; set; } = new List<ControlFactura>();

    public virtual TipoCambio? TipoCambio { get; set; }
}
