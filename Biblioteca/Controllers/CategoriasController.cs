using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Biblioteca.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly string _connectionString;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(IConfiguration configuration, ILogger<CategoriasController> logger)
        {
            _connectionString = configuration.GetConnectionString("BibliotecaConnection");
            _logger = logger;
        }

        // GET: Categorias
        public IActionResult Index()
        {
            _logger.LogInformation("Consultando el listado completo de categorías.");
            var categorias = new List<Categoria>();

            try
            {
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
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el listado de categorías.");
                throw;
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
            _logger.LogInformation("Iniciando creación de una nueva categoría: {Nombre}.", model?.Nombre);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("El modelo de la categoría a crear no es válido.");
                return View(model);
            }

            try
            {
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
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al crear la categoría {Nombre}.", model.Nombre);
                throw;
            }

            _logger.LogInformation("Categoría {Nombre} creada correctamente.", model.Nombre);
            return RedirectToAction(nameof(Index));
        }

        // GET: Categorias/Edit/5
        public IActionResult Edit(int id)
        {
            _logger.LogInformation("Iniciando edición de la categoría con ID {CategoriaId}.", id);
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

            if (categoria == null)
            {
                _logger.LogWarning("No se encontró la categoría con ID {CategoriaId} al intentar editarla.", id);
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Categoria model)
        {
            if (id != model.ID) return BadRequest();
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("El modelo de la categoría con ID {CategoriaId} no es válido al editar.", id);
                return View(model);
            }

            try
            {
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
                        if (filasAfectadas == 0)
                        {
                            _logger.LogWarning("No se encontró la categoría con ID {CategoriaId} al guardar la edición.", id);
                            return NotFound();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al editar la categoría con ID {CategoriaId}.", id);
                throw;
            }

            _logger.LogInformation("Categoría con ID {CategoriaId} actualizada correctamente.", id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Categorias/Delete/5
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Iniciando eliminación de la categoría con ID {CategoriaId}.", id);
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

            if (categoria == null)
            {
                _logger.LogWarning("No se encontró la categoría con ID {CategoriaId} al intentar eliminarla.", id);
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "DELETE FROM Categorias WHERE ID = @ID";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", id);

                        connection.Open();
                        var filasAfectadas = command.ExecuteNonQuery();
                        if (filasAfectadas == 0)
                        {
                            _logger.LogWarning("No se encontró la categoría con ID {CategoriaId} al confirmar la eliminación.", id);
                            return NotFound();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la categoría con ID {CategoriaId}.", id);
                throw;
            }

            _logger.LogInformation("Categoría con ID {CategoriaId} eliminada correctamente.", id);
            return RedirectToAction(nameof(Index));
        }
    }
}
