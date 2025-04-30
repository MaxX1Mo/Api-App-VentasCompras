using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_VentasCompras.Migrations
{
    /// <inheritdoc />
    public partial class fixcategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Categorias_IDCategoria",
                table: "Productos");

            migrationBuilder.AlterColumn<int>(
                name: "IDCategoria",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Categorias_IDCategoria",
                table: "Productos",
                column: "IDCategoria",
                principalTable: "Categorias",
                principalColumn: "IDCategoria",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Categorias_IDCategoria",
                table: "Productos");

            migrationBuilder.AlterColumn<int>(
                name: "IDCategoria",
                table: "Productos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Categorias_IDCategoria",
                table: "Productos",
                column: "IDCategoria",
                principalTable: "Categorias",
                principalColumn: "IDCategoria");
        }
    }
}
