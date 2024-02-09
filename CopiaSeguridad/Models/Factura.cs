using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Models;

public partial class Factura
{
    public string CodFactura { get; set; } = null!;

    public string? NumFactura { get; set; }

    public int? Monto { get; set; }

    [Range(1, 12, ErrorMessage = "Los meses deben ser del {1} al {2}.")]
    public byte? MesContable { get; set; }

    public short? AnioContable { get; set; }

    public string? Empresa { get; set; }

    public virtual ICollection<ControlFactura> ControlFacturas { get; set; } = new List<ControlFactura>();

    public virtual TipoCambio? TipoCambio { get; set; }
}
