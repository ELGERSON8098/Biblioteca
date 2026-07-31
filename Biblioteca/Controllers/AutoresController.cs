using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace Biblioteca.Controllers
{
    public class AutoresController : Controller
    {
        // Usamos una lista estática en memoria para facilitar el ejercicio.
        private static List<Autor> _autores = new List<Autor>
        {
            new Autor { ID = 1, Nombre = "Gabriel", Apellido = "García Márquez", Nacionalidad = "Colombiana", FechaNacimiento = new DateTime(1927,3,6), Activo = false },
            new Autor { ID = 2, Nombre = "Isabel", Apellido = "Allende", Nacionalidad = "Chilena", FechaNacimiento = new DateTime(1942,8,2), Activo = true },
            new Autor { ID = 3, Nombre = "Jorge Luis", Apellido = "Borges", Nacionalidad = "Argentina", FechaNacimiento = new DateTime(1899,8,24), Activo = false },
            new Autor { ID = 4, Nombre = "Mario", Apellido = "Vargas Llosa", Nacionalidad = "Peruana", FechaNacimiento = new DateTime(1936,3,28), Activo = true },
            new Autor { ID = 5, Nombre = "Laura", Apellido = "Restrepo", Nacionalidad = "Colombiana", FechaNacimiento = new DateTime(1950,5,1), Activo = true }
        };

        public IActionResult Index()
        {
            return View(_autores);
        }

        // GET: Autores/Edit/5
        public IActionResult Edit(int id)
        {
            var autor = _autores.Find(a => a.ID == id);
            if (autor == null) return NotFound();
            return View(autor);
        }

        // POST: Autores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Autor model)
        {
            if (id != model.ID) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var autor = _autores.Find(a => a.ID == id);
            if (autor == null) return NotFound();

            // Actualizar campos
            autor.Nombre = model.Nombre;
            autor.Apellido = model.Apellido;
            autor.Nacionalidad = model.Nacionalidad;
            autor.FechaNacimiento = model.FechaNacimiento;
            autor.Activo = model.Activo;

            return RedirectToAction(nameof(Index));
        }

        // GET: Autores/Delete/5
        public IActionResult Delete(int id)
        {
            var autor = _autores.Find(a => a.ID == id);
            if (autor == null) return NotFound();
            return View(autor);
        }

        // POST: Autores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var autor = _autores.Find(a => a.ID == id);
            if (autor == null) return NotFound();
            _autores.Remove(autor);
            return RedirectToAction(nameof(Index));
        }
    }
}
