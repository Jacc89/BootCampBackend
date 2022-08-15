using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructura.Data.Migrations
{
    public partial class MigrationInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TbCliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbCliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbEmpleado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sueldo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbEmpleado", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Caracteristica = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbProducto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbRemision",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumRemision = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", maxLength: 100, nullable: false),
                    EncargadoId = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    ClienteId = table.Column<int>(type: "int", maxLength: 150, nullable: false),
                    Cantidad = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Presentacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProductoId = table.Column<int>(type: "int", maxLength: 40, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbRemision", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TbRemision_TbCliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "TbCliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbRemision_TbEmpleado_EncargadoId",
                        column: x => x.EncargadoId,
                        principalTable: "TbEmpleado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TbRemision_TbProducto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "TbProducto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbRemision_ClienteId",
                table: "TbRemision",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRemision_EncargadoId",
                table: "TbRemision",
                column: "EncargadoId");

            migrationBuilder.CreateIndex(
                name: "IX_TbRemision_ProductoId",
                table: "TbRemision",
                column: "ProductoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TbRemision");

            migrationBuilder.DropTable(
                name: "TbCliente");

            migrationBuilder.DropTable(
                name: "TbEmpleado");

            migrationBuilder.DropTable(
                name: "TbProducto");
        }
    }
}
