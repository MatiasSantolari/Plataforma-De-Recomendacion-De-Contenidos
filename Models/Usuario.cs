using System;
using System.Collections.Generic;

namespace Plataforma_De_Recomendacion_De_Contenido.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Nacionalidad { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string Email { get; set; } = null!;

    public string Rol { get; set; } = null!;
}
