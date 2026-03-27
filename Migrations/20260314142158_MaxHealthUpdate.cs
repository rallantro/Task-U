using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_U.Migrations
{
    /// <inheritdoc />
    public partial class MaxHealthUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hp",
                table: "Personagens");

            migrationBuilder.RenameColumn(
                name: "Hp",
                table: "Inimigos",
                newName: "HpMax");

            migrationBuilder.AlterColumn<int>(
                name: "HpMax",
                table: "Personagens",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HpMax",
                table: "Inimigos",
                newName: "Hp");

            migrationBuilder.AlterColumn<int>(
                name: "HpMax",
                table: "Personagens",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "Hp",
                table: "Personagens",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
