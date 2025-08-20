using Microsoft.AspNetCore.Mvc.Rendering;

namespace Plataforma_De_Recomendacion_De_Contenido.Models.ViewModels
{
    public class SerieVM
    {
        // Datos de la película
        public int ContenidoId { get; set; }
        public string Nombre { get; set; }
        public string Sinopsis { get; set; }
        public string Pais { get; set; }
        public string Director { get; set; }
        public int AnoLanzamiento { get; set; }
        public int CantidadTemporadas { get; set; }

        // Lista de géneros disponibles para mostrar en el formulario
        public List<SelectListItem> GenerosDisponibles { get; set; } = new();

        // Ids de los géneros seleccionados en el formulario
        public List<int> GenerosSeleccionados { get; set; } = new();
    }
}
