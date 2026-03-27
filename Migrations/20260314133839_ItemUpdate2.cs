using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_U.Migrations
{
    /// <inheritdoc />
    public partial class ItemUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_PersonagemAtivoId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PersonagemAtivoId",
                table: "Users",
                column: "PersonagemAtivoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_PersonagemAtivoId",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PersonagemAtivoId",
                table: "Users",
                column: "PersonagemAtivoId");
        }
    }
}
