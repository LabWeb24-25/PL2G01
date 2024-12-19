using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteBiblioteca.Data.Migrations
{
    /// <inheritdoc />
    public partial class RequisitarV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "livroISBN",
                table: "requisicoes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_requisicoes_biblioEntregaId",
                table: "requisicoes",
                column: "biblioEntregaId");

            migrationBuilder.CreateIndex(
                name: "IX_requisicoes_biblioRecebeId",
                table: "requisicoes",
                column: "biblioRecebeId");

            migrationBuilder.CreateIndex(
                name: "IX_requisicoes_leitorId",
                table: "requisicoes",
                column: "leitorId");

            migrationBuilder.CreateIndex(
                name: "IX_requisicoes_livroISBN",
                table: "requisicoes",
                column: "livroISBN");

            migrationBuilder.AddForeignKey(
                name: "FK_requisicoes_Adicional_biblioEntregaId",
                table: "requisicoes",
                column: "biblioEntregaId",
                principalTable: "Adicional",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_requisicoes_Adicional_biblioRecebeId",
                table: "requisicoes",
                column: "biblioRecebeId",
                principalTable: "Adicional",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_requisicoes_Adicional_leitorId",
                table: "requisicoes",
                column: "leitorId",
                principalTable: "Adicional",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_requisicoes_livros_livroISBN",
                table: "requisicoes",
                column: "livroISBN",
                principalTable: "livros",
                principalColumn: "ISBN",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_requisicoes_Adicional_biblioEntregaId",
                table: "requisicoes");

            migrationBuilder.DropForeignKey(
                name: "FK_requisicoes_Adicional_biblioRecebeId",
                table: "requisicoes");

            migrationBuilder.DropForeignKey(
                name: "FK_requisicoes_Adicional_leitorId",
                table: "requisicoes");

            migrationBuilder.DropForeignKey(
                name: "FK_requisicoes_livros_livroISBN",
                table: "requisicoes");

            migrationBuilder.DropIndex(
                name: "IX_requisicoes_biblioEntregaId",
                table: "requisicoes");

            migrationBuilder.DropIndex(
                name: "IX_requisicoes_biblioRecebeId",
                table: "requisicoes");

            migrationBuilder.DropIndex(
                name: "IX_requisicoes_leitorId",
                table: "requisicoes");

            migrationBuilder.DropIndex(
                name: "IX_requisicoes_livroISBN",
                table: "requisicoes");

            migrationBuilder.DropColumn(
                name: "livroISBN",
                table: "requisicoes");
        }
    }
}
