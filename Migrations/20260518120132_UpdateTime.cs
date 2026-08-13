using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_U.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Speed",
                table: "Personagens",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Speed",
                table: "Inimigos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Inimigos");
        }
    }
}
