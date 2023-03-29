using System;
using System.Collections.Generic;

namespace ArmadillosManager.Models;

public partial class Categoria
{
    public int IdCategoria { get; set; }

    public string Categoria1 { get; set; } = null!;

    public virtual ICollection<Jugador> Jugador { get; } = new List<Jugador>();
}
