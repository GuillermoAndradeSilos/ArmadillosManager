namespace ArmadillosManager.Models.ViewModels
{
    public class AgregarJugadorViewModel
    {
        public Jugador Jugador { get; set; } = null!;
        public IEnumerable<Categoria> Categorias { get; set; } = null!;
        public IEnumerable<Liga> Ligas { get; set; } = null!;
        public IEnumerable<Responsable> Responsables { get; set; } = null!;
        public string? RFC { get; set; }
    }
}
