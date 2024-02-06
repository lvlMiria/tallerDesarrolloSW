using System;
using System.Collections.Generic;

namespace Presentacion.Models;

public partial class Concepto
{
    public byte CodConcepto { get; set; }

    public string? DescConcepto { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
