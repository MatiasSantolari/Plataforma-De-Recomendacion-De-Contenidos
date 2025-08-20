using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plataforma_De_Recomendacion_De_Contenido.Models;

namespace Plataforma_De_Recomendacion_De_Contenido.Controllers
{
    public class GeneroController : Controller
    {
        private readonly DbplataformaRecomendacionDeContenidoContext _context;
        public GeneroController(DbplataformaRecomendacionDeContenidoContext context)
        {
            _context = context;
        }

        // GET: GeneroController
        public ActionResult Index()
        {
            List<Genero> generos = _context.Generos.ToList();
            return View(generos);
        }

        // GET: GeneroController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GeneroController/Create
        public ActionResult Create(int idGenero)
        {
            Genero oGenero = new Genero()
            {
                Nombre = "Nombre por defecto",
                Detalle = "Detalle por defecto"
            };
            return View(oGenero);
        }

        // POST: GeneroController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Genero oGenero)
        {
            if (oGenero.GeneroId == 0)
            {
                _context.Generos.Add(oGenero);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Genero");
        }

        // GET: GeneroController/Edit/5
        public ActionResult Edit(int idGenero)
        {
            Genero oGenero = new Genero();
            if (idGenero != 0)
            {
                var g = _context.Generos.Find(idGenero);
                if (g != null)
                {
                    oGenero.GeneroId = idGenero;
                    oGenero.Nombre = g.Nombre;
                    oGenero.Detalle = g.Detalle;
                }
            }
            return View(oGenero);
        }

        // POST: GeneroController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Genero oGenero)
        {
            if (oGenero.GeneroId != 0)
            {
                _context.Generos.Update(oGenero);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Genero");
        }

        // GET: GeneroController/Delete/5
        public ActionResult Delete(int idGenero)
        {
            Genero oGenero = _context.Generos
                                     .Where(g => g.GeneroId == idGenero)
                                     .FirstOrDefault();
            return View(oGenero);
        }

        // POST: GeneroController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Genero oGenero)
        {
            // Borrar primero las relaciones en GeneroContenido
            var relaciones = _context.GeneroContenidos
                                     .Where(gc => gc.GeneroId == oGenero.GeneroId)
                                     .ToList();
            _context.GeneroContenidos.RemoveRange(relaciones);
            // Luego borrar el género en sí
            _context.Generos.Remove(oGenero);
            _context.SaveChanges();
            return RedirectToAction("Index", "Genero");
        }
    }
}
