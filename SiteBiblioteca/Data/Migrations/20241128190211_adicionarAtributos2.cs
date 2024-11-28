using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteBiblioteca.Data.Migrations
{
    /// <inheritdoc />
    public partial class adicionarAtributos2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tiktok",
                table: "_dadosBiblioteca",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tiktok",
                table: "_dadosBiblioteca");
        }
    }
}
