using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace Biblioteca.Controllers
{
    public class AutoresController : Controller
    {
        public IActionResult Index()
        {
            var autores = new List<Autor>
            {
                new Autor { ID = 1, Nombre = "Gabriel", Apellido = "García Márquez", Nacionalidad = "Colombiana", FechaNacimiento = new DateTime(1927,3,6), Activo = false },
                new Autor { ID = 2, Nombre = "Isabel", Apellido = "Allende", Nacionalidad = "Chilena", FechaNacimiento = new DateTime(1942,8,2), Activo = true },
                new Autor { ID = 3, Nombre = "Jorge Luis", Apellido = "Borges", Nacionalidad = "Argentina", FechaNacimiento = new DateTime(1899,8,24), Activo = false },
                new Autor { ID = 4, Nombre = "Mario", Apellido = "Vargas Llosa", Nacionalidad = "Peruana", FechaNacimiento = new DateTime(1936,3,28), Activo = true },
                new Autor { ID = 5, Nombre = "Laura", Apellido = "Restrepo", Nacionalidad = "Colombiana", FechaNacimiento = new DateTime(1950,5,1), Activo = true }
            };

            return View(autores);
        }
    }
}
