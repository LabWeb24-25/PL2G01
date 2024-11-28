using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteBiblioteca.Data.Migrations
{
    /// <inheritdoc />
    public partial class adicionarAtributos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "autores",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "facebook",
                table: "_dadosBiblioteca",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "instagram",
                table: "_dadosBiblioteca",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "mapa",
                table: "_dadosBiblioteca",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "x",
                table: "_dadosBiblioteca",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "youtube",
                table: "_dadosBiblioteca",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "autores");

            migrationBuilder.DropColumn(
                name: "facebook",
                table: "_dadosBiblioteca");

            migrationBuilder.DropColumn(
                name: "instagram",
                table: "_dadosBiblioteca");

            migrationBuilder.DropColumn(
                name: "mapa",
                table: "_dadosBiblioteca");

            migrationBuilder.DropColumn(
                name: "x",
                table: "_dadosBiblioteca");

            migrationBuilder.DropColumn(
                name: "youtube",
                table: "_dadosBiblioteca");
        }
    }
}
