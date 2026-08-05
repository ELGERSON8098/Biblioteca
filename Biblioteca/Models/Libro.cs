using System;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Libro
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Categoria { get; set; }
        [Display(Name = "Año publicación")]
        public int AnioPublicacion { get; set; }
        public string Descripcion { get; set; }
        // Ruta relativa a wwwroot, por ejemplo: /images/cover1.jpg
        public string ImagenUrl { get; set; }
    }
}
