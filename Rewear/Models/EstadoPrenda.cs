using System.ComponentModel.DataAnnotations;

namespace Rewear.Models
{
    public class EstadoPrenda
    {
        [Key]
        public decimal idEstadoPrenda { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public ICollection<Publicacion>? Publicaciones { get; set; }
    }
}
