namespace ArmadillosManager.Models.ViewModels
{
    public class AgregarResponsableViewModel
    {
        public Responsable Responsable { get; set; } = null!;
        public IEnumerable<Categoria> Categorias { get; set; } = null!;
        public IEnumerable<Jugador> Jugadores { get; set; } = null!;
        public IEnumerable<Liga> Ligas { get; set; } = null!;
    }
}
