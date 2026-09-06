using System.ComponentModel.DataAnnotations;

namespace Rewear.Models
{
    public class Categoria
    {
        [Key]
        public decimal idCategoria { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public ICollection<Publicacion>? Publicaciones { get; set; }
    }
}
