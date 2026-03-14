using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo_Gacha.Migrations
{
    /// <inheritdoc />
    public partial class Correcao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Apostador_BaseAtk",
                table: "Personagens",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Apostador_BonusDMG",
                table: "Personagens",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BaseAtk",
                table: "Personagens",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BonusDMG",
                table: "Personagens",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Personagens",
                type: "TEXT",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "MoonState",
                table: "Personagens",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Win",
                table: "Personagens",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apostador_BaseAtk",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Apostador_BonusDMG",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "BaseAtk",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "BonusDMG",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "MoonState",
                table: "Personagens");

            migrationBuilder.DropColumn(
                name: "Win",
                table: "Personagens");
        }
    }
}
