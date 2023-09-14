using ArmadillosManager.Models;
using ArmadillosManager.Models.ViewModels;

namespace ArmadillosManager.Areas.Responsables.Models
{
    public class GenerarPDFModel
    {
        public IEnumerable<ResponsableHelpViewModel>? Movimientos { get; set; }
    }
}
