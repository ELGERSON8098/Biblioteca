using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Biblioteca.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly string _connectionString;

        public CategoriasController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BibliotecaConnection");
        }

        // GET: Categorias
        public IActionResult Index()
        {
            var categorias = new List<Categoria>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT ID, Nombre, Descripcion FROM Categorias ORDER BY Nombre";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categorias.Add(new Categoria
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion"))
                            });
                        }
                    }
                }
            }

            return View(categorias);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria model)
        {
            if (!ModelState.IsValid) return View(model);

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Categorias (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", model.Nombre);
                    command.Parameters.AddWithValue("@Descripcion", (object)model.Descripcion ?? System.DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Categorias/Edit/5
        public IActionResult Edit(int id)
        {
            Categoria categoria = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT ID, Nombre, Descripcion FROM Categorias WHERE ID = @ID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            categoria = new Categoria
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion"))
                            };
                        }
                    }
                }
            }

            if (categoria == null) return NotFound();
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Categoria model)
        {
            if (id != model.ID) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE Categorias SET Nombre = @Nombre, Descripcion = @Descripcion WHERE ID = @ID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", model.Nombre);
                    command.Parameters.AddWithValue("@Descripcion", (object)model.Descripcion ?? System.DBNull.Value);
                    command.Parameters.AddWithValue("@ID", model.ID);

                    connection.Open();
                    var filasAfectadas = command.ExecuteNonQuery();
                    if (filasAfectadas == 0) return NotFound();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Categorias/Delete/5
        public IActionResult Delete(int id)
        {
            Categoria categoria = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT ID, Nombre, Descripcion FROM Categorias WHERE ID = @ID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            categoria = new Categoria
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion"))
                            };
                        }
                    }
                }
            }

            if (categoria == null) return NotFound();
            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Categorias WHERE ID = @ID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();
                    var filasAfectadas = command.ExecuteNonQuery();
                    if (filasAfectadas == 0) return NotFound();
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
