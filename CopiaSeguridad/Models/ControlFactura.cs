using System;
using System.Collections.Generic;

namespace Presentacion.Models;

public partial class ControlFactura
{
    public int CodControl { get; set; }

    public int? CodPresupuesto { get; set; }

    public int? CodFactura { get; set; }

    public string? Origen { get; set; }

    public DateTime? FechaRecepcion { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public string? Comentario { get; set; }

    public virtual Factura? CodFacturaNavigation { get; set; }

    public virtual Presupuesto? CodPresupuestoNavigation { get; set; }
}
