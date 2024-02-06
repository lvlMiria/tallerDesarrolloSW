using System;
using System.Collections.Generic;

namespace Presentacion.Models;

public partial class Ipc
{
    public short Anio { get; set; }

    public decimal? Valor { get; set; }

    public virtual ICollection<Presupuesto> Presupuestos { get; set; } = new List<Presupuesto>();
}
