using System;
using System.Collections.Generic;

namespace Plataforma_De_Recomendacion_De_Contenido.Models;

public partial class UsuarioContenido
{
    public int UsuarioContenidoId { get; set; }

    public int UsuarioId { get; set; }

    public int ContenidoId { get; set; }

    public int? Calificacion { get; set; }

    public bool MarcadorFavoritos { get; set; }

    public virtual Contenido Contenido { get; set; } = null!;
}
