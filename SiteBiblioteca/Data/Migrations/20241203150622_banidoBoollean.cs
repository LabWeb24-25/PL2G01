using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteBiblioteca.Data.Migrations
{
    /// <inheritdoc />
    public partial class banidoBoollean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "banido",
                table: "Adicional",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "banido",
                table: "Adicional");
        }
    }
}
