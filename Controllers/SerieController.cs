using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Plataforma_De_Recomendacion_De_Contenido.Models;
using Plataforma_De_Recomendacion_De_Contenido.Models.ViewModels;

namespace Plataforma_De_Recomendacion_De_Contenido.Controllers
{
    public class SerieController : Controller
    {
        private readonly DbplataformaRecomendacionDeContenidoContext _context;

        public SerieController(DbplataformaRecomendacionDeContenidoContext context)
        {
            _context = context;
        }
        // GET: SerieController
        public ActionResult Index()
        {
            var series = _context.Series
                .ToList();

            return View(series);
        }

        // GET: SerieController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SerieController/Create
        public ActionResult Create()
        {
            var vm = new SerieVM
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

        // POST: SerieController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SerieVM vm)
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

            var serie = new Serie
            {
                // Propiedades heredadas (Contenido)
                Nombre = vm.Nombre,
                Sinopsis = vm.Sinopsis,
                Pais = vm.Pais,
                Director = vm.Director,
                AnoLanzamiento = vm.AnoLanzamiento,

                // Propiedades propias de Serie
                CantidadTemporadas = vm.CantidadTemporadas
            };

            _context.Series.Add(serie);
            _context.SaveChanges(); // EF inserta en Contenido + Serie

            // Insertar relaciones en GeneroContenido
            foreach (var generoId in vm.GenerosSeleccionados)
            {
                _context.GeneroContenidos.Add(new GeneroContenido
                {
                    ContenidoId = serie.ContenidoId, // ya generado
                    GeneroId = generoId
                });
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        // GET: SerieController/Edit/5
        public ActionResult Edit(int idSerie)
        {
            var oSerie = _context.Series
                .FirstOrDefault(s => s.ContenidoId == idSerie);

            if (oSerie == null) return NotFound();

            var vm = new SerieVM
            {
                ContenidoId = oSerie.ContenidoId,
                Nombre = oSerie.Nombre,
                Sinopsis = oSerie.Sinopsis,
                Pais = oSerie.Pais,
                Director = oSerie.Director,
                AnoLanzamiento = oSerie.AnoLanzamiento,
                CantidadTemporadas = oSerie.CantidadTemporadas,
                GenerosSeleccionados = _context.GeneroContenidos
                    .Where(gc => gc.ContenidoId == idSerie)
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

        // POST: SerieController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SerieVM vm)
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

            var serie = _context.Series.Find(vm.ContenidoId);
            if (serie == null) return NotFound();

            // Propiedades heredadas (Contenido)
            serie.Nombre = vm.Nombre;
            serie.Sinopsis = vm.Sinopsis;
            serie.Pais = vm.Pais;
            serie.Director = vm.Director;
            serie.AnoLanzamiento = vm.AnoLanzamiento;

            // Propiedades propias de Serie
            serie.CantidadTemporadas = vm.CantidadTemporadas;

            // Actualizar géneros (borramos y volvemos a insertar)
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

        // GET: SerieController/Delete/5
        public ActionResult Delete(int idSerie)
        {
            var oSerie = _context.Series
                .FirstOrDefault(s => s.ContenidoId == idSerie);

            var generosSeleccionados = _context.GeneroContenidos
                                               .Where(gc => gc.ContenidoId == oSerie.ContenidoId)
                                               .Select(gc => gc.GeneroId)
                                               .ToList();

            var oSerieVM = new SerieVM()
            {
                ContenidoId = oSerie.ContenidoId,
                Nombre = oSerie.Nombre,
                Sinopsis = oSerie.Sinopsis,
                Pais = oSerie.Pais,
                Director = oSerie.Director,
                AnoLanzamiento = oSerie.AnoLanzamiento,
                CantidadTemporadas = oSerie.CantidadTemporadas,
                GenerosDisponibles = _context.Generos
                    .Select(g => new SelectListItem
                    {
                        Value = g.GeneroId.ToString(),
                        Text = g.Nombre
                    }).ToList(),
                GenerosSeleccionados = generosSeleccionados,
            };
            return View(oSerieVM);
        }

        // POST: SerieController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(SerieVM vm)
        {
            var serie = _context.Series.Find(vm.ContenidoId);
            if (serie == null) return NotFound();

            // Borrar relaciones con géneros
            var relacionesGeneroContenido = _context.GeneroContenidos
                .Where(gc => gc.ContenidoId == serie.ContenidoId)
                .ToList();
            _context.GeneroContenidos.RemoveRange(relacionesGeneroContenido);

            // Borrar relaciones con usuarios
            var relacionesUsuarioContenido = _context.UsuarioContenidos
                .Where(uc => uc.ContenidoId == serie.ContenidoId)
                .ToList();
            _context.UsuarioContenidos.RemoveRange(relacionesUsuarioContenido);

            // Eliminar serie (EF borra también el Contenido)
            _context.Series.Remove(serie);

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
