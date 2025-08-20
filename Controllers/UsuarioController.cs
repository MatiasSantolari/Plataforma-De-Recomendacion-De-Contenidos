using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plataforma_De_Recomendacion_De_Contenido.Models;

namespace Plataforma_De_Recomendacion_De_Contenido.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly DbplataformaRecomendacionDeContenidoContext _context;
        public UsuarioController(DbplataformaRecomendacionDeContenidoContext context)
        {
            _context = context;
        }
        // GET: UsuarioController
        public ActionResult Index()
        {
            List<Usuario> usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }

        // GET: UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsuarioController/Create
        public ActionResult Create(int idUsuario)
        {
            Usuario oUsuario = new Usuario()
            {
                NombreUsuario = "Username por defecto",
                Email = "email por defecto",
                FechaNacimiento = DateOnly.FromDateTime(DateTime.Today),
                Nacionalidad = "Argentina",
                Rol = "Rol por defecto",
                Password = "Password por defecto",
            };
            return View(oUsuario);
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario oUsuario)
        {
            if (oUsuario.UsuarioId == 0)
            {
                _context.Usuarios.Add(oUsuario);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Usuario");
        }


        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int idUsuario)
        {
            Usuario oUsuario = new Usuario();
            if (idUsuario != 0)
            {
                var u
                    = _context.Usuarios.Find(idUsuario);
                if (u != null)
                {
                    oUsuario.UsuarioId = idUsuario;
                    oUsuario.NombreUsuario = u.NombreUsuario;
                    oUsuario.Email = u.Email;
                    oUsuario.FechaNacimiento = u.FechaNacimiento;
                    oUsuario.Nacionalidad = u.Nacionalidad;
                    oUsuario.Rol = u.Rol;
                    oUsuario.Password = u.Password;

                }
            }
            return View(oUsuario);
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usuario oUsuario)
        {
            if (oUsuario.UsuarioId != 0)
            {
                _context.Usuarios.Update(oUsuario);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Usuario");
        }

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int idUsuario)
        {
            Usuario oUsuario = _context.Usuarios
                                     .Where(u => u.UsuarioId == idUsuario)
                                     .FirstOrDefault();
            return View(oUsuario);
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Usuario oUsuario)
        {
            // Borramos primero las relaciones en UsuarioContenido
            var relaciones = _context.UsuarioContenidos
                                     .Where(gc => gc.UsuarioId == oUsuario.UsuarioId)
                                     .ToList();
            _context.UsuarioContenidos.RemoveRange(relaciones);
            // Luego borramos el género en sí
            _context.Usuarios.Remove(oUsuario);
            _context.SaveChanges();
            return RedirectToAction("Index", "Usuario");
        }
    }
}
