using System;
using System.Collections.Generic;

namespace Plataforma_De_Recomendacion_De_Contenido.Models;

public partial class Genero
{
    public int GeneroId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Detalle { get; set; } = null!;
}
