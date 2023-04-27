using System;
using System.Collections.Generic;

namespace ArmadillosManager.Models;

public partial class Temporada
{
    public int Id { get; set; }

    public DateOnly InicioLigaInfantil { get; set; }

    public DateOnly FinalLigaInfantil { get; set; }

    public DateOnly InicioLigaJuvenil { get; set; }

    public DateOnly FinalLigaJuvenil { get; set; }

    public decimal CostoTemporadaInfantil { get; set; }

    public decimal CostoUniformeInfantil { get; set; }

    public decimal CostoTemporadaJuvenil { get; set; }

    public decimal CostoUniformeJuvenil { get; set; }

    public virtual ICollection<Pago> Pago { get; } = new List<Pago>();
}
