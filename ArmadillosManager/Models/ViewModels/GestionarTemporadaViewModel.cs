namespace ArmadillosManager.Models.ViewModels
{
    public class GestionarTemporadaViewModel
    {
        public Temporada Temporada { get; set; } = null!;
        public string InicioLigaInfantilFormateada { get; set; } = null!;
        public string FinalLigaInfantilFormateada { get; set; } = null!;
        public string InicioLigaJuvenilFormateada { get; set; } = null!;
        public string FinalLigaJuvenilFormateada { get; set; } = null!;
    }
}
