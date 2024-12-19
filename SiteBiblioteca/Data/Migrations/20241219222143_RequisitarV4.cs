using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteBiblioteca.Data.Migrations
{
    /// <inheritdoc />
    public partial class RequisitarV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_requisicoes_Adicional_biblioEntregaId",
                table: "requisicoes");

            migrationBuilder.DropForeignKey(
                name: "FK_requisicoes_Adicional_biblioRecebeId",
                table: "requisicoes");

            migrationBuilder.DropForeignKey(
                name: "FK_requisicoes_livros_livroISBN",
                table: "requisicoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_requisicoes",
                table: "requisicoes");

            migrationBuilder.DropIndex(
                name: "IX_requisicoes_livroISBN",
                table: "requisicoes");

            migrationBuilder.DropColumn(
                name: "livroId",
                table: "requisicoes");

            migrationBuilder.AlterColumn<int>(
                name: "livroISBN",
                table: "requisicoes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "biblioRecebeId",
                table: "requisicoes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "biblioEntregaId",
                table: "requisicoes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "livroISBN1",
                table: "requisicoes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_requisicoes",
                table: "requisicoes",
                columns: new[] { "livroISBN", "leitorId", "data_requisicao" });

            migrationBuilder.CreateIndex(
                name: "IX_requisicoes_livroISBN1",
                table: "requisicoes",
                column: "livroISBN1");

            migrationBuilder.AddForeignKey(
                name: "FK_requisicoes_Adicional_biblioEntregaId",
                table: "requisicoes",
                column: "biblioEntregaId",
                principalTable: "Adicional",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_requisicoes_Adicional_biblioRecebeId",
                table: "requisicoes",
                column: "biblioRecebeId",
                principalTable: "Adicional",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_requisicoes_livros_livroISBN1",
                table: "requisicoes",
                column: "livroISBN1",
                principalTable: "livros",
                principalColumn: "ISBN");
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
                name: "FK_requisicoes_livros_livroISBN1",
                table: "requisicoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_requisicoes",
                table: "requisicoes");

            migrationBuilder.DropIndex(
                name: "IX_requisicoes_livroISBN1",
                table: "requisicoes");

            migrationBuilder.DropColumn(
                name: "livroISBN1",
                table: "requisicoes");

            migrationBuilder.AlterColumn<int>(
                name: "biblioRecebeId",
                table: "requisicoes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "biblioEntregaId",
                table: "requisicoes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "livroISBN",
                table: "requisicoes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "livroId",
                table: "requisicoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_requisicoes",
                table: "requisicoes",
                columns: new[] { "livroId", "leitorId", "data_requisicao" });

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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_requisicoes_Adicional_biblioRecebeId",
                table: "requisicoes",
                column: "biblioRecebeId",
                principalTable: "Adicional",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_requisicoes_livros_livroISBN",
                table: "requisicoes",
                column: "livroISBN",
                principalTable: "livros",
                principalColumn: "ISBN");
        }
    }
}
