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

    public decimal CostoTemporada { get; set; }

    public decimal CostoUniforme { get; set; }
}
