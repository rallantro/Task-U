using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_U.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarDiscriminanteItens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Itens",
                type: "TEXT",
                maxLength: 13,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Itens");
        }
    }
}
