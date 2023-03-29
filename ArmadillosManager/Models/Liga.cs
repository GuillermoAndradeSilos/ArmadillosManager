using System;
using System.Collections.Generic;

namespace ArmadillosManager.Models;

public partial class Liga
{
    public int IdLiga { get; set; }

    public string Liga1 { get; set; } = null!;

    public virtual ICollection<Jugador> Jugador { get; } = new List<Jugador>();
}
