using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

namespace Biblioteca.Controllers
{
    public class LibrosController : Controller
    {
        private readonly IWebHostEnvironment _env;

        // Lista en memoria para el ejemplo
        private static List<Libro> _libros = new List<Libro>
        {
            new Libro { ID = 1, Titulo = "Cien Años de Soledad", Autor = "Gabriel García Márquez", Categoria = "Novela", AnioPublicacion = 1967, Descripcion = "Clásico de la literatura latinoamericana.", ImagenUrl = null },
            new Libro { ID = 2, Titulo = "La ciudad y los perros", Autor = "Mario Vargas Llosa", Categoria = "Novela", AnioPublicacion = 1963, Descripcion = "Novela sobre la vida en un colegio militar.", ImagenUrl = null }
        };

        public LibrosController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // GET: Libros
        public IActionResult Index()
        {
            return View(_libros);
        }

        // GET: Libros/Details/5
        public IActionResult Details(int id)
        {
            var libro = _libros.Find(l => l.ID == id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        // GET: Libros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Libros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Libro model, IFormFile imagen)
        {
            if (!ModelState.IsValid) return View(model);

            // Guardar imagen si se proporcionó
            if (imagen != null && imagen.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "images");
                Directory.CreateDirectory(uploads);
                var ext = Path.GetExtension(imagen.FileName);
                var fileName = Guid.NewGuid().ToString() + ext;
                var filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imagen.CopyTo(stream);
                }
                model.ImagenUrl = "/images/" + fileName;
            }

            // Asignar ID
            model.ID = _libros.Any() ? _libros.Max(l => l.ID) + 1 : 1;
            _libros.Add(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: Libros/Edit/5
        public IActionResult Edit(int id)
        {
            var libro = _libros.Find(l => l.ID == id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        // POST: Libros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Libro model, IFormFile imagen)
        {
            if (id != model.ID) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var libro = _libros.Find(l => l.ID == id);
            if (libro == null) return NotFound();

            // Si se sube nueva imagen, reemplazar
            if (imagen != null && imagen.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "images");
                Directory.CreateDirectory(uploads);
                var ext = Path.GetExtension(imagen.FileName);
                var fileName = Guid.NewGuid().ToString() + ext;
                var filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imagen.CopyTo(stream);
                }
                model.ImagenUrl = "/images/" + fileName;
            }

            // Actualizar propiedades
            libro.Titulo = model.Titulo;
            libro.Autor = model.Autor;
            libro.Categoria = model.Categoria;
            libro.AnioPublicacion = model.AnioPublicacion;
            libro.Descripcion = model.Descripcion;
            if (!string.IsNullOrEmpty(model.ImagenUrl)) libro.ImagenUrl = model.ImagenUrl;

            return RedirectToAction(nameof(Index));
        }

        // GET: Libros/Delete/5
        public IActionResult Delete(int id)
        {
            var libro = _libros.Find(l => l.ID == id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var libro = _libros.Find(l => l.ID == id);
            if (libro == null) return NotFound();
            _libros.Remove(libro);
            return RedirectToAction(nameof(Index));
        }
    }
}
