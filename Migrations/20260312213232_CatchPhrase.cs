using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo_Gacha.Migrations
{
    /// <inheritdoc />
    public partial class CatchPhrase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SummonQuote",
                table: "Personagens",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SummonQuote",
                table: "Personagens");
        }
    }
}
