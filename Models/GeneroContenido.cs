using System;
using System.Collections.Generic;

namespace Plataforma_De_Recomendacion_De_Contenido.Models;

public partial class GeneroContenido
{
    public int GeneroContenidoId { get; set; }

    public int GeneroId { get; set; }

    public int ContenidoId { get; set; }
}
