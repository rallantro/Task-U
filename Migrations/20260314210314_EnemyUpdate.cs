using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo_Gacha.Migrations
{
    /// <inheritdoc />
    public partial class EnemyUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DerrotouInimigo",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InimigoId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rarity",
                table: "Inimigos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DerrotouInimigo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "InimigoId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Rarity",
                table: "Inimigos");
        }
    }
}
