﻿namespace ArmadillosManager.Models.ViewModels
{
    public class GestionarResponsablesViewModel
    {
        public IEnumerable<Responsable> Responsables { get; set; } = null!;
        public Filtros? Filtros { get; set; }
        public IEnumerable<Categoria> Categorias { get; set; } = null!;
    }
}
