using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_U.Migrations
{
    /// <inheritdoc />
    public partial class TeamsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Personagens_PersonagemAtivoId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "PersonagemAtivoId",
                table: "Users",
                newName: "Slot2_PersonagemAtivoId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_PersonagemAtivoId",
                table: "Users",
                newName: "IX_Users_Slot2_PersonagemAtivoId");

            migrationBuilder.AddColumn<int>(
                name: "Slot1_PersonagemAtivoId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Inimigos",
                type: "TEXT",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Slot1_PersonagemAtivoId",
                table: "Users",
                column: "Slot1_PersonagemAtivoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Personagens_Slot1_PersonagemAtivoId",
                table: "Users",
                column: "Slot1_PersonagemAtivoId",
                principalTable: "Personagens",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Personagens_Slot2_PersonagemAtivoId",
                table: "Users",
                column: "Slot2_PersonagemAtivoId",
                principalTable: "Personagens",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Personagens_Slot1_PersonagemAtivoId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Personagens_Slot2_PersonagemAtivoId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Slot1_PersonagemAtivoId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Slot1_PersonagemAtivoId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Inimigos");

            migrationBuilder.RenameColumn(
                name: "Slot2_PersonagemAtivoId",
                table: "Users",
                newName: "PersonagemAtivoId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Slot2_PersonagemAtivoId",
                table: "Users",
                newName: "IX_Users_PersonagemAtivoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Personagens_PersonagemAtivoId",
                table: "Users",
                column: "PersonagemAtivoId",
                principalTable: "Personagens",
                principalColumn: "Id");
        }
    }
}
