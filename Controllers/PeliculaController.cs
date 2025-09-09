using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Plataforma_De_Recomendacion_De_Contenido.Models;
using Plataforma_De_Recomendacion_De_Contenido.Models.ViewModels;
using System.Linq;

namespace Plataforma_De_Recomendacion_De_Contenido.Controllers
{
    public class PeliculaController : Controller
    {
        private readonly DbplataformaRecomendacionDeContenidoContext _context;

        public PeliculaController(DbplataformaRecomendacionDeContenidoContext context)
        {
            _context = context;
        }

        // GET: PeliculaController
        public ActionResult Index()
        {
            var peliculas = _context.Peliculas
                .ToList();

            return View(peliculas);
        }

        // GET: PeliculaController/Create
        public ActionResult Create()
        {
            var vm = new PeliculaVM
            {
                GenerosDisponibles = _context.Generos
                    .Select(g => new SelectListItem
                    {
                        Value = g.GeneroId.ToString(),
                        Text = g.Nombre
                    }).ToList()
            };

            return View(vm);
        }

        // POST: PeliculaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PeliculaVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.GenerosDisponibles = _context.Generos
                    .Select(g => new SelectListItem
                    {
                        Value = g.GeneroId.ToString(),
                        Text = g.Nombre
                    }).ToList();
                return View(vm);
            }

            var pelicula = new Pelicula
            {
                Nombre = vm.Nombre,
                Sinopsis = vm.Sinopsis,
                Pais = vm.Pais,
                Director = vm.Director,
                AnoLanzamiento = vm.AnoLanzamiento,
                DuracionMinutos = vm.DuracionMinutos
            };

            _context.Peliculas.Add(pelicula);
            _context.SaveChanges();

            // Insertar relaciones en GeneroContenido
            foreach (var generoId in vm.GenerosSeleccionados)
            {
                _context.GeneroContenidos.Add(new GeneroContenido
                {
                    ContenidoId = pelicula.ContenidoId,
                    GeneroId = generoId
                });
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: PeliculaController/Edit/5
        public ActionResult Edit(int idPelicula)
        {
            var oPelicula = _context.Peliculas
                .FirstOrDefault(p => p.ContenidoId == idPelicula);

            if (oPelicula == null) return NotFound();

            var vm = new PeliculaVM
            {
                ContenidoId = oPelicula.ContenidoId,
                Nombre = oPelicula.Nombre,
                Sinopsis = oPelicula.Sinopsis,
                Pais = oPelicula.Pais,
                Director = oPelicula.Director,
                AnoLanzamiento = oPelicula.AnoLanzamiento,
                DuracionMinutos = oPelicula.DuracionMinutos,
                GenerosSeleccionados = _context.GeneroContenidos
                    .Where(gc => gc.ContenidoId == idPelicula)
                    .Select(gc => gc.GeneroId)
                    .ToList(),
                GenerosDisponibles = _context.Generos
                    .Select(g => new SelectListItem
                    {
                        Value = g.GeneroId.ToString(),
                        Text = g.Nombre
                    }).ToList()
            };

            return View(vm);
        }

        // POST: PeliculaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PeliculaVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.GenerosDisponibles = _context.Generos
                    .Select(g => new SelectListItem
                    {
                        Value = g.GeneroId.ToString(),
                        Text = g.Nombre
                    }).ToList();
                return View(vm);
            }

            var pelicula = _context.Peliculas.Find(vm.ContenidoId);
            if (pelicula == null) return NotFound();

            // Actualizar propiedades heredadas (Contenido)
            pelicula.Nombre = vm.Nombre;
            pelicula.Sinopsis = vm.Sinopsis;
            pelicula.Pais = vm.Pais;
            pelicula.Director = vm.Director;
            pelicula.AnoLanzamiento = vm.AnoLanzamiento;

            // Actualizar propiedades propias de Pelicula
            pelicula.DuracionMinutos = vm.DuracionMinutos;

            // Actualizar géneros (limpiamos y volvemos a insertar)
            var generosExistentes = _context.GeneroContenidos
                .Where(gc => gc.ContenidoId == vm.ContenidoId);
            _context.GeneroContenidos.RemoveRange(generosExistentes);

            foreach (var generoId in vm.GenerosSeleccionados)
            {
                _context.GeneroContenidos.Add(new GeneroContenido
                {
                    ContenidoId = vm.ContenidoId,
                    GeneroId = generoId
                });
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: PeliculaController/Delete/5
        public ActionResult Delete(int idPelicula)
        {
            var oPelicula = _context.Peliculas
                .FirstOrDefault(p => p.ContenidoId == idPelicula);

            var generosSeleccionados = _context.GeneroContenidos
                                               .Where(gc => gc.ContenidoId == oPelicula.ContenidoId)
                                               .Select(gc => gc.GeneroId)
                                               .ToList();

            var oPeliculaVM = new PeliculaVM()
            {
                ContenidoId = oPelicula.ContenidoId,
                Nombre = oPelicula.Nombre,
                Sinopsis = oPelicula.Sinopsis,
                Pais = oPelicula.Pais,
                Director = oPelicula.Director,
                AnoLanzamiento = oPelicula.AnoLanzamiento,
                DuracionMinutos = oPelicula.DuracionMinutos,
                GenerosDisponibles = _context.Generos
                    .Select(g => new SelectListItem
                    {
                        Value = g.GeneroId.ToString(),
                        Text = g.Nombre
                    }).ToList(),
                GenerosSeleccionados = generosSeleccionados,
            };
            return View(oPeliculaVM);
        }

        // POST: PeliculaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PeliculaVM vm)
        {
            var pelicula = _context.Peliculas.Find(vm.ContenidoId);
            if (pelicula == null) return NotFound();

            // Borrar relaciones con géneros
            var relacionesGeneroContenido = _context.GeneroContenidos
                .Where(gc => gc.ContenidoId == pelicula.ContenidoId)
                .ToList();
            _context.GeneroContenidos.RemoveRange(relacionesGeneroContenido);

            // Borrar relaciones con usuarios
            var relacionesUsuarioContenido = _context.UsuarioContenidos
                .Where(uc => uc.ContenidoId == pelicula.ContenidoId)
                .ToList();
            _context.UsuarioContenidos.RemoveRange(relacionesUsuarioContenido);

            // Eliminar película (esto debería eliminar también Contenido)
            _context.Peliculas.Remove(pelicula);

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult GetPelicula(int idPelicula)
        {
            var pelicula = _context.Peliculas
                                   .Include(p => p.UsuarioContenidos)
                                   .FirstOrDefault(p => p.ContenidoId == idPelicula);
            if (pelicula == null) return BadRequest(error: 404);
            return View(pelicula);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarFavorito(int contenidoId)
        {
            int usuarioId = 1; // TODO: tomar el id del usuario logueado

            var usuarioContenido = _context.UsuarioContenidos
                .FirstOrDefault(uc => uc.ContenidoId == contenidoId && uc.UsuarioId == usuarioId); //esto del usuario esta harcodeado de momento

            if (usuarioContenido == null)
            {
                usuarioContenido = new UsuarioContenido()
                {
                    ContenidoId = contenidoId,
                    UsuarioId = 1, //tambien hardcodeado
                    MarcadorFavoritos = true,
                    Calificacion = null,
                };
                _context.UsuarioContenidos.Add(usuarioContenido);
            }
            else
            {
                usuarioContenido.MarcadorFavoritos = !usuarioContenido.MarcadorFavoritos; //le cambio el valor que ya tiene
                _context.UsuarioContenidos.Update(usuarioContenido);
            }
            _context.SaveChanges();

            return RedirectToAction("GetPelicula", new { idPelicula = contenidoId });
        }
    }
}
