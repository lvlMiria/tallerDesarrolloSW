using System;
using System.Collections.Generic;

namespace Presentacion.Models;

public partial class Item
{
    public short CodItem { get; set; }

    public string? DescItem { get; set; }

    public string? GastoInversion { get; set; }

    public string? ContNuevo { get; set; }

    public byte? CodConcepto { get; set; }

    public byte? Estado { get; set; }

    public virtual Concepto? CodConceptoNavigation { get; set; }

    public virtual ICollection<Presupuesto> Presupuestos { get; set; } = new List<Presupuesto>();
}
