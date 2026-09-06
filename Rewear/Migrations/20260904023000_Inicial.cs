using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rewear.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    idCategoria = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.idCategoria);
                });

            migrationBuilder.CreateTable(
                name: "EstadosPrenda",
                columns: table => new
                {
                    idEstadoPrenda = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosPrenda", x => x.idEstadoPrenda);
                });

            migrationBuilder.CreateTable(
                name: "Publicaciones",
                columns: table => new
                {
                    idPublicacion = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Imagen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vendedor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoPrenda_idEstadoPrenda = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Categoria_idCategoria = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicaciones", x => x.idPublicacion);
                    table.ForeignKey(
                        name: "FK_Publicaciones_Categorias_Categoria_idCategoria",
                        column: x => x.Categoria_idCategoria,
                        principalTable: "Categorias",
                        principalColumn: "idCategoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Publicaciones_EstadosPrenda_EstadoPrenda_idEstadoPrenda",
                        column: x => x.EstadoPrenda_idEstadoPrenda,
                        principalTable: "EstadosPrenda",
                        principalColumn: "idEstadoPrenda",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Publicaciones_Categoria_idCategoria",
                table: "Publicaciones",
                column: "Categoria_idCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Publicaciones_EstadoPrenda_idEstadoPrenda",
                table: "Publicaciones",
                column: "EstadoPrenda_idEstadoPrenda");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Publicaciones");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "EstadosPrenda");
        }
    }
}
