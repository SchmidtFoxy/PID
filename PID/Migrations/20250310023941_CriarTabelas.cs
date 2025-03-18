using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PID.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Desenvolvimentos",
                columns: table => new
                {
                    IdDesenvolvimento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Classificacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dificuldade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Produto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ERP = table.Column<int>(type: "int", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjetoFinep = table.Column<bool>(type: "bit", nullable: false),
                    ProjetoLeiBem = table.Column<bool>(type: "bit", nullable: false),
                    Custo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fase = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TempoDesenvolvimento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Solicitante = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Desenvolvimentos", x => x.IdDesenvolvimento);
                });

            migrationBuilder.CreateTable(
                name: "Dispendios",
                columns: table => new
                {
                    IdDispendio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjetoFinep = table.Column<bool>(type: "bit", nullable: false),
                    ProjetoLeiBem = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispendios", x => x.IdDispendio);
                });

            migrationBuilder.CreateTable(
                name: "Historicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdDesenvolvimento = table.Column<int>(type: "int", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Responsavel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historicos_Desenvolvimentos_IdDesenvolvimento",
                        column: x => x.IdDesenvolvimento,
                        principalTable: "Desenvolvimentos",
                        principalColumn: "IdDesenvolvimento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tarefas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdDesenvolvimento = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prioridade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarefas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tarefas_Desenvolvimentos_IdDesenvolvimento",
                        column: x => x.IdDesenvolvimento,
                        principalTable: "Desenvolvimentos",
                        principalColumn: "IdDesenvolvimento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Custos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdDispendio = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdDesenvolvimento = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<float>(type: "real", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Custos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Custos_Desenvolvimentos_IdDesenvolvimento",
                        column: x => x.IdDesenvolvimento,
                        principalTable: "Desenvolvimentos",
                        principalColumn: "IdDesenvolvimento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Custos_Dispendios_IdDispendio",
                        column: x => x.IdDispendio,
                        principalTable: "Dispendios",
                        principalColumn: "IdDispendio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetosPD",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ano = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdDesenvolvimento = table.Column<int>(type: "int", nullable: false),
                    IdDispendio = table.Column<int>(type: "int", nullable: false),
                    ProjetoFinep = table.Column<bool>(type: "bit", nullable: false),
                    ProjetoLeiBem = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosPD", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetosPD_Desenvolvimentos_IdDesenvolvimento",
                        column: x => x.IdDesenvolvimento,
                        principalTable: "Desenvolvimentos",
                        principalColumn: "IdDesenvolvimento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjetosPD_Dispendios_IdDispendio",
                        column: x => x.IdDispendio,
                        principalTable: "Dispendios",
                        principalColumn: "IdDispendio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Custos_IdDesenvolvimento",
                table: "Custos",
                column: "IdDesenvolvimento");

            migrationBuilder.CreateIndex(
                name: "IX_Custos_IdDispendio",
                table: "Custos",
                column: "IdDispendio");

            migrationBuilder.CreateIndex(
                name: "IX_Historicos_IdDesenvolvimento",
                table: "Historicos",
                column: "IdDesenvolvimento");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosPD_IdDesenvolvimento",
                table: "ProjetosPD",
                column: "IdDesenvolvimento");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosPD_IdDispendio",
                table: "ProjetosPD",
                column: "IdDispendio");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_IdDesenvolvimento",
                table: "Tarefas",
                column: "IdDesenvolvimento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Custos");

            migrationBuilder.DropTable(
                name: "Historicos");

            migrationBuilder.DropTable(
                name: "ProjetosPD");

            migrationBuilder.DropTable(
                name: "Tarefas");

            migrationBuilder.DropTable(
                name: "Dispendios");

            migrationBuilder.DropTable(
                name: "Desenvolvimentos");
        }
    }
}
