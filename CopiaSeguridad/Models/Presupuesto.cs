using System;
using System.Collections.Generic;

namespace Presentacion.Models;

public partial class Presupuesto
{
    public int CodPresupuesto { get; set; }

    public byte? Mes { get; set; }

    public short? Anio { get; set; }

    public int? PresupuestoMes { get; set; }

    public short? CodItem { get; set; }

    public virtual Item? CodItemNavigation { get; set; }

    public virtual ICollection<ControlFactura> ControlFacturas { get; set; } = new List<ControlFactura>();
}
