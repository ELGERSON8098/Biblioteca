using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace Biblioteca.Controllers
{
    public class AutoresController : Controller
    {
        private readonly Biblioteca.Services.IAutorService _autorService;

        public AutoresController(Biblioteca.Services.IAutorService autorService)
        {
            _autorService = autorService;
        }

        public IActionResult Index()
        {
            var autores = _autorService.GetAll();
            return View(autores);
        }

        // GET: Autores/Edit/5
        public IActionResult Edit(int id)
        {
            var autor = _autorService.GetById(id);
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

            var autor = _autorService.GetById(id);
            if (autor == null) return NotFound();

            _autorService.Update(model);

            return RedirectToAction(nameof(Index));
        }

        // GET: Autores/Delete/5
        public IActionResult Delete(int id)
        {
            var autor = _autorService.GetById(id);
            if (autor == null) return NotFound();
            return View(autor);
        }

        // POST: Autores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var autor = _autorService.GetById(id);
            if (autor == null) return NotFound();
            _autorService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
