using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Models;

public partial class Ipc
{
    public short Anio { get; set; }

    [ModelBinder(BinderType = typeof(MBDecimal))]
    [Display(Name = "Valor")]
    public decimal? Valor { get; set; }

    public virtual ICollection<Presupuesto> Presupuestos { get; set; } = new List<Presupuesto>();
}
