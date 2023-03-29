using System;
using System.Collections.Generic;

namespace ArmadillosManager.Models;

public partial class Jugador
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public int Categoria { get; set; }

    public int Liga { get; set; }

    public int IdResponsable { get; set; }

    public virtual Categoria CategoriaNavigation { get; set; } = null!;

    public virtual Responsable IdResponsableNavigation { get; set; } = null!;

    public virtual Liga LigaNavigation { get; set; } = null!;

    public virtual ICollection<Pago> Pago { get; } = new List<Pago>();
}
