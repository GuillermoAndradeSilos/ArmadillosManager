using System;
using System.Collections.Generic;

namespace ArmadillosManager.Models;

public partial class Pago
{
    public int Id { get; set; }

    public int IdResponsable { get; set; }

    public int IdJugador { get; set; }

    public decimal MontoTotal { get; set; }

    public decimal MontoRestante { get; set; }

    public int IdTemporada { get; set; }

    public string? Estado { get; set; }

    public virtual Jugador IdJugadorNavigation { get; set; } = null!;

    public virtual Responsable IdResponsableNavigation { get; set; } = null!;

    public virtual Temporada IdTemporadaNavigation { get; set; } = null!;

    public virtual ICollection<Movimientos> Movimientos { get; } = new List<Movimientos>();
}
