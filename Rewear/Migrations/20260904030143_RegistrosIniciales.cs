using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rewear.Migrations
{
    /// <inheritdoc />
    public partial class RegistrosIniciales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "idCategoria", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1m, "Ropa y prendas para mujer", "Mujer" },
                    { 2m, "Ropa y prendas para hombre", "Hombre" },
                    { 3m, "Calzado de segunda mano", "Zapatos" },
                    { 4m, "Bolsos, carteras y otros accesorios", "Accesorios" }
                });

            migrationBuilder.InsertData(
                table: "EstadosPrenda",
                columns: new[] { "idEstadoPrenda", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1m, "Prenda sin uso", "Nuevo" },
                    { 2m, "Prenda usada pocas veces", "Como nuevo" },
                    { 3m, "Prenda usada pero bien conservada", "Buen estado" },
                    { 4m, "Prenda con signos de uso", "Usado" }
                });

            migrationBuilder.InsertData(
                table: "Publicaciones",
                columns: new[] { "idPublicacion", "Categoria_idCategoria", "Descripcion", "EstadoPrenda_idEstadoPrenda", "Imagen", "Precio", "Titulo", "Vendedor" },
                values: new object[,]
                {
                    { 1m, 1m, "Blazer en buen estado, talla M.", 3m, "/images/blazer.jpg", 65.00m, "Blazer verde menta", "María" },
                    { 2m, 1m, "Suéter tejido color beige, talla M.", 2m, "/images/sueter.jpg", 45.00m, "Suéter tejido beige", "Ana" },
                    { 3m, 1m, "Jeans azul de corte mom fit, talla 28.", 3m, "/images/jeans.jpg", 55.00m, "Jeans mom fit", "Carla" },
                    { 4m, 3m, "Zapatillas blancas en buen estado, talla 38.", 3m, "/images/zapatillas.jpg", 80.00m, "Zapatillas blancas", "Luis" },
                    { 5m, 4m, "Cartera marrón de segunda mano.", 2m, "/images/cartera.jpg", 60.00m, "Cartera marrón", "Sofía" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "idCategoria",
                keyValue: 2m);

            migrationBuilder.DeleteData(
                table: "EstadosPrenda",
                keyColumn: "idEstadoPrenda",
                keyValue: 1m);

            migrationBuilder.DeleteData(
                table: "EstadosPrenda",
                keyColumn: "idEstadoPrenda",
                keyValue: 4m);

            migrationBuilder.DeleteData(
                table: "Publicaciones",
                keyColumn: "idPublicacion",
                keyValue: 1m);

            migrationBuilder.DeleteData(
                table: "Publicaciones",
                keyColumn: "idPublicacion",
                keyValue: 2m);

            migrationBuilder.DeleteData(
                table: "Publicaciones",
                keyColumn: "idPublicacion",
                keyValue: 3m);

            migrationBuilder.DeleteData(
                table: "Publicaciones",
                keyColumn: "idPublicacion",
                keyValue: 4m);

            migrationBuilder.DeleteData(
                table: "Publicaciones",
                keyColumn: "idPublicacion",
                keyValue: 5m);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "idCategoria",
                keyValue: 1m);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "idCategoria",
                keyValue: 3m);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "idCategoria",
                keyValue: 4m);

            migrationBuilder.DeleteData(
                table: "EstadosPrenda",
                keyColumn: "idEstadoPrenda",
                keyValue: 2m);

            migrationBuilder.DeleteData(
                table: "EstadosPrenda",
                keyColumn: "idEstadoPrenda",
                keyValue: 3m);
        }
    }
}
