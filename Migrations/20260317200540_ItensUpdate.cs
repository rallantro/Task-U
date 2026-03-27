using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_U.Migrations
{
    /// <inheritdoc />
    public partial class ItensUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Itens_ItemAtivoId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ItemAtivoId",
                table: "Users",
                newName: "Slot2_ItemAtivoId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_ItemAtivoId",
                table: "Users",
                newName: "IX_Users_Slot2_ItemAtivoId");

            migrationBuilder.AddColumn<int>(
                name: "Slot1_ItemAtivoId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Slot1_ItemAtivoId",
                table: "Users",
                column: "Slot1_ItemAtivoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Itens_Slot1_ItemAtivoId",
                table: "Users",
                column: "Slot1_ItemAtivoId",
                principalTable: "Itens",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Itens_Slot2_ItemAtivoId",
                table: "Users",
                column: "Slot2_ItemAtivoId",
                principalTable: "Itens",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Itens_Slot1_ItemAtivoId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Itens_Slot2_ItemAtivoId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Slot1_ItemAtivoId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Slot1_ItemAtivoId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Slot2_ItemAtivoId",
                table: "Users",
                newName: "ItemAtivoId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Slot2_ItemAtivoId",
                table: "Users",
                newName: "IX_Users_ItemAtivoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Itens_ItemAtivoId",
                table: "Users",
                column: "ItemAtivoId",
                principalTable: "Itens",
                principalColumn: "Id");
        }
    }
}
