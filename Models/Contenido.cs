using System;
using System.Collections.Generic;

namespace Plataforma_De_Recomendacion_De_Contenido.Models;

public partial class Contenido
{
    public int ContenidoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Sinopsis { get; set; } = null!;

    public string Pais { get; set; } = null!;

    public string Director { get; set; } = null!;

    public int AnoLanzamiento { get; set; }

    public virtual ICollection<UsuarioContenido> UsuarioContenidos { get; set; } = new List<UsuarioContenido>();
}
