using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PID.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCamposProjetoPDNovamente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Classificacao",
                table: "ProjetosPD",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Componente",
                table: "ProjetosPD",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "ProjetosPD",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Estrutura",
                table: "ProjetosPD",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Solicitante",
                table: "ProjetosPD",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TipoProduto",
                table: "ProjetosPD",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Classificacao",
                table: "ProjetosPD");

            migrationBuilder.DropColumn(
                name: "Componente",
                table: "ProjetosPD");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "ProjetosPD");

            migrationBuilder.DropColumn(
                name: "Estrutura",
                table: "ProjetosPD");

            migrationBuilder.DropColumn(
                name: "Solicitante",
                table: "ProjetosPD");

            migrationBuilder.DropColumn(
                name: "TipoProduto",
                table: "ProjetosPD");
        }
    }
}
