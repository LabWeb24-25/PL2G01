using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteBiblioteca.Data.Migrations
{
    /// <inheritdoc />
    public partial class others : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_dadosBiblioteca",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contactos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    horario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mapa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    x = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    instagram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    youtube = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tiktok = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__dadosBiblioteca", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Adicional",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    confirmado = table.Column<bool>(type: "bit", nullable: false),
                    banido = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adicional", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "autores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bibliografia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Imagem = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_autores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bloqueios",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false),
                    adminId = table.Column<int>(type: "int", nullable: false),
                    dataBloqueio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    motivo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bloqueios", x => new { x.userId, x.adminId, x.dataBloqueio });
                });

            migrationBuilder.CreateTable(
                name: "livros",
                columns: table => new
                {
                    ISBN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    autorId = table.Column<int>(type: "int", nullable: false),
                    genero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    preco = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    numExemplares = table.Column<int>(type: "int", nullable: false),
                    sinopse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imagem = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_livros", x => x.ISBN);
                    table.ForeignKey(
                        name: "FK_livros_autores_autorId",
                        column: x => x.autorId,
                        principalTable: "autores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "requisicoes",
                columns: table => new
                {
                    leitorId = table.Column<int>(type: "int", nullable: false),
                    livroISBN = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    data_requisicao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    data_entrega = table.Column<DateTime>(type: "datetime2", nullable: false),
                    biblioEntregaId = table.Column<int>(type: "int", nullable: true),
                    biblioRecebeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_requisicoes", x => new { x.livroISBN, x.leitorId, x.data_requisicao });
                    table.ForeignKey(
                        name: "FK_requisicoes_Adicional_biblioEntregaId",
                        column: x => x.biblioEntregaId,
                        principalTable: "Adicional",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_requisicoes_Adicional_biblioRecebeId",
                        column: x => x.biblioRecebeId,
                        principalTable: "Adicional",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_requisicoes_Adicional_leitorId",
                        column: x => x.leitorId,
                        principalTable: "Adicional",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_requisicoes_livros_livroISBN",
                        column: x => x.livroISBN,
                        principalTable: "livros",
                        principalColumn: "ISBN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_livros_autorId",
                table: "livros",
                column: "autorId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_dadosBiblioteca");

            migrationBuilder.DropTable(
                name: "bloqueios");

            migrationBuilder.DropTable(
                name: "requisicoes");

            migrationBuilder.DropTable(
                name: "Adicional");

            migrationBuilder.DropTable(
                name: "livros");

            migrationBuilder.DropTable(
                name: "autores");
        }
    }
}
