namespace ArmadillosManager.Models.ViewModels
{
    public class ResponsableViewModel
    {
        public Responsable ResponsableInfo { get; set; } = null!;
        public IEnumerable<Jugador> Jugadores { get; set; }
        public IEnumerable<ResponsableHelpViewModel> Movimientos { get; set; }
    }
}
