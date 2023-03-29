using System;
using System.Collections.Generic;

namespace ArmadillosManager.Models;

public partial class Abono
{
    public int Id { get; set; }

    public int IdPago { get; set; }

    public DateTime Fecha { get; set; }

    public string Monto { get; set; } = null!;

    public virtual Pago IdPagoNavigation { get; set; } = null!;
}
