using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_VentasCompras.Migrations
{
    /// <inheritdoc />
    public partial class PrimerMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    IDCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.IDCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Ubicaciones",
                columns: table => new
                {
                    IDUbicacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoPostal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Localidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Provincia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Calle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NroCalle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ubicaciones", x => x.IDUbicacion);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IDUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Rol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IDUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    IDPersona = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NroCelular = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IDUbicacion = table.Column<int>(type: "int", nullable: false),
                    IDUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.IDPersona);
                    table.ForeignKey(
                        name: "FK_Personas_Ubicaciones_IDUbicacion",
                        column: x => x.IDUbicacion,
                        principalTable: "Ubicaciones",
                        principalColumn: "IDUbicacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Personas_Usuarios_IDUsuario",
                        column: x => x.IDUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IDUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    IDProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreProducto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Imagen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IDUsuario = table.Column<int>(type: "int", nullable: false),
                    IDCategoria = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.IDProducto);
                    table.ForeignKey(
                        name: "FK_Productos_Categorias_IDCategoria",
                        column: x => x.IDCategoria,
                        principalTable: "Categorias",
                        principalColumn: "IDCategoria");
                    table.ForeignKey(
                        name: "FK_Productos_Usuarios_IDUsuario",
                        column: x => x.IDUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IDUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    IDStatus = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VentasExitosas = table.Column<int>(type: "int", nullable: true),
                    IDUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.IDStatus);
                    table.ForeignKey(
                        name: "FK_Status_Usuarios_IDUsuario",
                        column: x => x.IDUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IDUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductoVentas",
                columns: table => new
                {
                    IDProductoVenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoProducto = table.Column<int>(type: "int", nullable: false),
                    EstadoVenta = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    IDProducto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoVentas", x => x.IDProductoVenta);
                    table.ForeignKey(
                        name: "FK_ProductoVentas_Productos_IDProducto",
                        column: x => x.IDProducto,
                        principalTable: "Productos",
                        principalColumn: "IDProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Valoraciones",
                columns: table => new
                {
                    IDValoraciones = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bueno = table.Column<int>(type: "int", nullable: true),
                    Malo = table.Column<int>(type: "int", nullable: true),
                    Regular = table.Column<int>(type: "int", nullable: true),
                    IDStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Valoraciones", x => x.IDValoraciones);
                    table.ForeignKey(
                        name: "FK_Valoraciones_Status_IDStatus",
                        column: x => x.IDStatus,
                        principalTable: "Status",
                        principalColumn: "IDStatus",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personas_IDUbicacion",
                table: "Personas",
                column: "IDUbicacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personas_IDUsuario",
                table: "Personas",
                column: "IDUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_IDCategoria",
                table: "Productos",
                column: "IDCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_IDUsuario",
                table: "Productos",
                column: "IDUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoVentas_IDProducto",
                table: "ProductoVentas",
                column: "IDProducto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Status_IDUsuario",
                table: "Status",
                column: "IDUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Valoraciones_IDStatus",
                table: "Valoraciones",
                column: "IDStatus",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personas");

            migrationBuilder.DropTable(
                name: "ProductoVentas");

            migrationBuilder.DropTable(
                name: "Valoraciones");

            migrationBuilder.DropTable(
                name: "Ubicaciones");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
