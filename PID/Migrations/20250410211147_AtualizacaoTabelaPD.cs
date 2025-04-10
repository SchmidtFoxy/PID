using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PID.Migrations
{
    /// <inheritdoc />
    public partial class AtualizacaoTabelaPD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "ProjetosPD",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Titulo",
                table: "ProjetosPD",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "ProjetosPD");

            migrationBuilder.DropColumn(
                name: "Titulo",
                table: "ProjetosPD");
        }
    }
}
