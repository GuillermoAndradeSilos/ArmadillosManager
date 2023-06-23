using System;
using System.Collections.Generic;

namespace ArmadillosManager.Models;

public partial class Responsable
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Rfc { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string? Contraseña { get; set; }

    public virtual ICollection<Jugador> Jugador { get; } = new List<Jugador>();

    public virtual ICollection<Pago> Pago { get; } = new List<Pago>();
}
