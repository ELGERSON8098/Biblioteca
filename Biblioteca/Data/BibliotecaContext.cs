using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Biblioteca.Data
{
    // Extend IdentityDbContext so Identity tables are stored in the same database
    public class BibliotecaContext : IdentityDbContext<IdentityUser>
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
            : base(options)
        {
        }

        public DbSet<Libro> Libros { get; set; }
    }
}
