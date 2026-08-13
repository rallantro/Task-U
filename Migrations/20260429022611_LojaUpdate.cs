using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_U.Migrations
{
    /// <inheritdoc />
    public partial class LojaUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "personagem",
                table: "loja",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "exclLoja",
                table: "Itens",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "personagem",
                table: "loja");

            migrationBuilder.DropColumn(
                name: "exclLoja",
                table: "Itens");
        }
    }
}
