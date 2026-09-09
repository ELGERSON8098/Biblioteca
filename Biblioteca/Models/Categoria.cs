using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Categoria
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(300)]
        public string Descripcion { get; set; }
    }
}
