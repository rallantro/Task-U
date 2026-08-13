using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_U.Migrations
{
    /// <inheritdoc />
    public partial class BossType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "tipoBoss",
                table: "Inimigos",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tipoBoss",
                table: "Inimigos");
        }
    }
}
