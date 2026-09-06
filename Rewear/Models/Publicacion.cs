using System.ComponentModel.DataAnnotations;

namespace Rewear.Models
{
    public class Publicacion
    {
        [Key]
        public decimal idPublicacion { get; set; }

        public string? Titulo { get; set; }

        public decimal? Precio { get; set; }

        public string? Descripcion { get; set; }

        public string? Imagen { get; set; }

        public string? Vendedor { get; set; }

        public decimal EstadoPrenda_idEstadoPrenda { get; set; }

        public decimal Categoria_idCategoria { get; set; }

        public Categoria? Categoria { get; set; }

        public EstadoPrenda? EstadoPrenda { get; set; }
    }
}
