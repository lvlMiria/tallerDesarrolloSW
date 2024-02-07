using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentacion.Models;

public partial class ControlFactura
{
    public string CodControl { get; set; } = null!;

    public string? CodPresupuesto { get; set; }

    public string? CodFactura { get; set; }

    public string? Origen { get; set; }

    public DateTime? FechaRecepcion { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public string? Comentario { get; set; }

    public virtual Factura? CodFacturaNavigation { get; set; }

    public virtual Presupuesto? CodPresupuestoNavigation { get; set; }
}
