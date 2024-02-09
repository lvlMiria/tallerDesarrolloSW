using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Models;

public partial class Presupuesto
{
    public string CodPresupuesto { get; set; } = null!;

    [Range(1,12, ErrorMessage = "Los meses deben ser del {1} al {2}.")]
    public byte? Mes { get; set; }

    public short? Anio { get; set; }

    public int? PresupuestoMes { get; set; }

    public string? CodItem { get; set; }

    public virtual Ipc? AnioNavigation { get; set; }

    public virtual Item? CodItemNavigation { get; set; }

    public virtual ICollection<ControlFactura> ControlFacturas { get; set; } = new List<ControlFactura>();
}
