using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rewear.Migrations
{
    /// <inheritdoc />
    public partial class CrearUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contrasena = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Apellidos", "Contrasena", "Correo", "Estado", "FechaRegistro", "Nombres", "Telefono" },
                values: new object[,]
                {
                    { 1, "López Ramírez", "clave1234", "maria.lopez@example.com", true, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "María", "987654321" },
                    { 2, "Torres Vega", "clave1234", "ana.torres@example.com", true, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ana", "987654322" },
                    { 3, "Fernández Ríos", "clave1234", "luis.fernandez@example.com", false, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Luis", "987654323" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
