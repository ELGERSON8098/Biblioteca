using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Biblioteca.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Biblioteca.Controllers
{
    [Authorize]
    public class LibrosController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly BibliotecaContext _context;
        private readonly ILogger<LibrosController> _logger;

        public LibrosController(IWebHostEnvironment env, BibliotecaContext context, ILogger<LibrosController> logger)
        {
            _env = env;
            _context = context;
            _logger = logger;
        }

        // GET: Libros
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Consultando el listado completo de libros.");
            var libros = await _context.Libros.ToListAsync();
            return View(libros);
        }

        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Consultando el detalle del libro con ID {LibroId}.", id);
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                _logger.LogWarning("No se encontró el libro con ID {LibroId} al consultar el detalle.", id);
                return NotFound();
            }
            return View(libro);
        }

        // GET: Libros/Create
        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Libros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotecario")]
        public async Task<IActionResult> Create(Libro model, IFormFile imagen)
        {
            _logger.LogInformation("Iniciando creación de un nuevo libro: {Titulo}.", model?.Titulo);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("El modelo del libro a crear no es válido.");
                return View(model);
            }

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

            _logger.LogInformation("Libro creado correctamente con ID {LibroId}.", model.ID);
            TempData["Success"] = "Libro agregado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Libros/Edit/5
        [Authorize(Roles = "Admin,Bibliotecario")]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Iniciando edición del libro con ID {LibroId}.", id);
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                _logger.LogWarning("No se encontró el libro con ID {LibroId} al intentar editarlo.", id);
                return NotFound();
            }
            return View(libro);
        }

        // POST: Libros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotecario")]
        public async Task<IActionResult> Edit(int id, Libro model, IFormFile imagen)
        {
            if (id != model.ID) return BadRequest();
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("El modelo del libro con ID {LibroId} no es válido al editar.", id);
                return View(model);
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                _logger.LogWarning("No se encontró el libro con ID {LibroId} al guardar la edición.", id);
                return NotFound();
            }

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

            _logger.LogInformation("Libro con ID {LibroId} actualizado correctamente.", id);
            TempData["Success"] = "Libro actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Libros/Delete/5
        [Authorize(Roles = "Admin,Bibliotecario")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Iniciando eliminación del libro con ID {LibroId}.", id);
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                _logger.LogWarning("No se encontró el libro con ID {LibroId} al intentar eliminarlo.", id);
                return NotFound();
            }
            return View(libro);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotecario")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                _logger.LogWarning("No se encontró el libro con ID {LibroId} al confirmar la eliminación.", id);
                return NotFound();
            }
            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Libro con ID {LibroId} eliminado correctamente.", id);
            TempData["Success"] = "Libro eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
