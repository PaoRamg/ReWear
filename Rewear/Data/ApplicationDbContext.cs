using Microsoft.EntityFrameworkCore;
using Rewear.Models;

namespace Rewear.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Publicacion> Publicaciones { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<EstadoPrenda> EstadosPrenda { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Relaciones de la tabla Publicacion con las otras tablas
            modelBuilder.Entity<Publicacion>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Publicaciones)
                .HasForeignKey(p => p.Categoria_idCategoria);

            modelBuilder.Entity<Publicacion>()
                .HasOne(p => p.EstadoPrenda)
                .WithMany(e => e.Publicaciones)
                .HasForeignKey(p => p.EstadoPrenda_idEstadoPrenda);

            //Registros iniciales para la tabla Categoria
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria
                {
                    idCategoria = 1,
                    Nombre = "Mujer",
                    Descripcion = "Ropa y prendas para mujer"
                },
                new Categoria
                {
                    idCategoria = 2,
                    Nombre = "Hombre",
                    Descripcion = "Ropa y prendas para hombre"
                },
                new Categoria
                {
                    idCategoria = 3,
                    Nombre = "Zapatos",
                    Descripcion = "Calzado de segunda mano"
                },
                new Categoria
                {
                    idCategoria = 4,
                    Nombre = "Accesorios",
                    Descripcion = "Bolsos, carteras y otros accesorios"
                }
            );

            //Registros iniciales para la tabla Estado de Prenda
            modelBuilder.Entity<EstadoPrenda>().HasData(
                new EstadoPrenda
                {
                    idEstadoPrenda = 1,
                    Nombre = "Nuevo",
                    Descripcion = "Prenda sin uso"
                },
                new EstadoPrenda
                {
                    idEstadoPrenda = 2,
                    Nombre = "Como nuevo",
                    Descripcion = "Prenda usada pocas veces"
                },
                new EstadoPrenda
                {
                    idEstadoPrenda = 3,
                    Nombre = "Buen estado",
                    Descripcion = "Prenda usada pero bien conservada"
                },
                new EstadoPrenda
                {
                    idEstadoPrenda = 4,
                    Nombre = "Usado",
                    Descripcion = "Prenda con signos de uso"
                }
            );

            //Registros iniciales para la tabla Publicacion
            modelBuilder.Entity<Publicacion>().HasData(
                new Publicacion
                {
                    idPublicacion = 1,
                    Titulo = "Blazer verde menta",
                    Precio = 65.00m,//"m" indica que es un decimal
                    Descripcion = "Blazer en buen estado, talla M.",
                    Imagen = "/images/blazer.jpg",
                    Vendedor = "María",
                    Categoria_idCategoria = 1,
                    EstadoPrenda_idEstadoPrenda = 3
                },
                new Publicacion
                {
                    idPublicacion = 2,
                    Titulo = "Suéter tejido beige",
                    Precio = 45.00m,
                    Descripcion = "Suéter tejido color beige, talla M.",
                    Imagen = "/images/sueter.jpg",
                    Vendedor = "Ana",
                    Categoria_idCategoria = 1,
                    EstadoPrenda_idEstadoPrenda = 2
                },
                new Publicacion
                {
                    idPublicacion = 3,
                    Titulo = "Jeans mom fit",
                    Precio = 55.00m,
                    Descripcion = "Jeans azul de corte mom fit, talla 28.",
                    Imagen = "/images/jeans.jpg",
                    Vendedor = "Carla",
                    Categoria_idCategoria = 1,
                    EstadoPrenda_idEstadoPrenda = 3
                },
                new Publicacion
                {
                    idPublicacion = 4,
                    Titulo = "Zapatillas blancas",
                    Precio = 80.00m,
                    Descripcion = "Zapatillas blancas en buen estado, talla 38.",
                    Imagen = "/images/zapatillas.jpg",
                    Vendedor = "Luis",
                    Categoria_idCategoria = 3,
                    EstadoPrenda_idEstadoPrenda = 3
                },
                new Publicacion
                {
                    idPublicacion = 5,
                    Titulo = "Cartera marrón",
                    Precio = 60.00m,
                    Descripcion = "Cartera marrón de segunda mano.",
                    Imagen = "/images/cartera.jpg",
                    Vendedor = "Sofía",
                    Categoria_idCategoria = 4,
                    EstadoPrenda_idEstadoPrenda = 2
                }
            );

            //Registros iniciales para la tabla Usuario (entidad independiente)
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Nombres = "María",
                    Apellidos = "López Ramírez",
                    Correo = "maria.lopez@example.com",
                    Contrasena = "clave1234",
                    Telefono = "987654321",
                    FechaRegistro = new DateTime(2026, 1, 10),
                    Estado = true
                },
                new Usuario
                {
                    Id = 2,
                    Nombres = "Ana",
                    Apellidos = "Torres Vega",
                    Correo = "ana.torres@example.com",
                    Contrasena = "clave1234",
                    Telefono = "987654322",
                    FechaRegistro = new DateTime(2026, 2, 15),
                    Estado = true
                },
                new Usuario
                {
                    Id = 3,
                    Nombres = "Luis",
                    Apellidos = "Fernández Ríos",
                    Correo = "luis.fernandez@example.com",
                    Contrasena = "clave1234",
                    Telefono = "987654323",
                    FechaRegistro = new DateTime(2026, 3, 5),
                    Estado = false
                }
            );
        }
    }
}
