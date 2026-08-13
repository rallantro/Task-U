using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_U.Migrations
{
    /// <inheritdoc />
    public partial class ConstelacoesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Bits",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLojaUpdate",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "NodesNivel",
                table: "InventarioPersonagens",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantidade",
                table: "InventarioPersonagens",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "loja",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    personagemId = table.Column<int>(type: "INTEGER", nullable: false),
                    preco = table.Column<int>(type: "INTEGER", nullable: false),
                    comprado = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loja", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "loja");

            migrationBuilder.DropColumn(
                name: "Bits",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLojaUpdate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NodesNivel",
                table: "InventarioPersonagens");

            migrationBuilder.DropColumn(
                name: "Quantidade",
                table: "InventarioPersonagens");
        }
    }
}
