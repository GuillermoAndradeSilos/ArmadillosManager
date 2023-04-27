namespace ArmadillosManager.Models.ViewModels
{
    //enum Estados { Pendiente, Pagado }
    public class GestionarPagoViewModel
    {
        public IEnumerable<Pago>? Pagos { get; set; }
        public Filtros? Filtros { get; set; }
        public IEnumerable<Categoria> Categorias { get; set; } = null!;
    }
}
