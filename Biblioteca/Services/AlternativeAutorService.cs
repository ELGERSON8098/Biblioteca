using System.Collections.Generic;
using System.Linq;
using System;
using Biblioteca.Models;

namespace Biblioteca.Services
{
    // Segunda implementación de IAutorService (reto). Devuelve la misma colección
    // pero en orden inverso para demostrar que el controlador no necesita cambios.
    public class AlternativeAutorService : IAutorService
    {
        private static readonly List<Autor> _autoresAlt = new List<Autor>
        {
            new Autor { ID = 1, Nombre = "Gabriel", Apellido = "García Márquez", Nacionalidad = "Colombiana", FechaNacimiento = new DateTime(1927,3,6), Activo = false },
            new Autor { ID = 2, Nombre = "Isabel", Apellido = "Allende", Nacionalidad = "Chilena", FechaNacimiento = new DateTime(1942,8,2), Activo = true },
            new Autor { ID = 3, Nombre = "Jorge Luis", Apellido = "Borges", Nacionalidad = "Argentina", FechaNacimiento = new DateTime(1899,8,24), Activo = false },
            new Autor { ID = 4, Nombre = "Mario", Apellido = "Vargas Llosa", Nacionalidad = "Peruana", FechaNacimiento = new DateTime(1936,3,28), Activo = true },
            new Autor { ID = 5, Nombre = "Laura", Apellido = "Restrepo", Nacionalidad = "Colombiana", FechaNacimiento = new DateTime(1950,5,1), Activo = true }
        };

        public IEnumerable<Autor> GetAll()
        {
            return _autoresAlt.OrderByDescending(a => a.ID).ToList();
        }

        public Autor GetById(int id)
        {
            return _autoresAlt.FirstOrDefault(a => a.ID == id);
        }

        public void Update(Autor autor)
        {
            var existing = _autoresAlt.FirstOrDefault(a => a.ID == autor.ID);
            if (existing == null) return;

            existing.Nombre = autor.Nombre;
            existing.Apellido = autor.Apellido;
            existing.Nacionalidad = autor.Nacionalidad;
            existing.FechaNacimiento = autor.FechaNacimiento;
            existing.Activo = autor.Activo;
        }

        public void Delete(int id)
        {
            var existing = _autoresAlt.FirstOrDefault(a => a.ID == id);
            if (existing != null) _autoresAlt.Remove(existing);
        }
    }
}
