using System;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Libro
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200)]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El autor es obligatorio")]
        [StringLength(150)]
        public string Autor { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [StringLength(100)]
        public string Categoria { get; set; }

        [Display(Name = "Año publicación")]
        [Range(1000, 2100, ErrorMessage = "Ingrese un año válido")]
        public int AnioPublicacion { get; set; }

        [StringLength(2000)]
        public string Descripcion { get; set; }
        // Ruta relativa a wwwroot, por ejemplo: /images/cover1.jpg
        public string ImagenUrl { get; set; }
    }
}
