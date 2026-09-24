using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Biblioteca.Data;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    public class LibrosController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly BibliotecaContext _context;

        public LibrosController(IWebHostEnvironment env, BibliotecaContext context)
        {
            _env = env;
            _context = context;
        }

        // GET: Libros
        public async Task<IActionResult> Index()
        {
            var libros = await _context.Libros.ToListAsync();
            return View(libros);
        }

        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
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
        public async Task<IActionResult> Create(Libro model, IFormFile imagen)
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
                    await imagen.CopyToAsync(stream);
                }
                model.ImagenUrl = "/images/" + fileName;
            }

            // Agregar a la base de datos
            _context.Libros.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Libro agregado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        // POST: Libros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Libro model, IFormFile imagen)
        {
            if (id != model.ID) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var libro = await _context.Libros.FindAsync(id);
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
                    await imagen.CopyToAsync(stream);
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

            _context.Libros.Update(libro);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Libro actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null) return NotFound();
            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Libro eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
