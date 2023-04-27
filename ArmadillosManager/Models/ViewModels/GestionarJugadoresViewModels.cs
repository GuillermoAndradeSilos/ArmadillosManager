namespace ArmadillosManager.Models.ViewModels
{
    public class GestionarJugadoresViewModels
    {
        public IEnumerable<Jugador> Jugadores { get; set; } = null!;
        public Filtros? Filtros { get; set; }
        public IEnumerable<Categoria> Categorias { get; set; } = null!;
    }
}
