using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PID.Migrations
{
    /// <inheritdoc />
    public partial class MudaCalculoCustoDesenvolvimentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DesenvolvimentoIdDesenvolvimento",
                table: "Custos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Custos_DesenvolvimentoIdDesenvolvimento",
                table: "Custos",
                column: "DesenvolvimentoIdDesenvolvimento");

            migrationBuilder.AddForeignKey(
                name: "FK_Custos_Desenvolvimentos_DesenvolvimentoIdDesenvolvimento",
                table: "Custos",
                column: "DesenvolvimentoIdDesenvolvimento",
                principalTable: "Desenvolvimentos",
                principalColumn: "IdDesenvolvimento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Custos_Desenvolvimentos_DesenvolvimentoIdDesenvolvimento",
                table: "Custos");

            migrationBuilder.DropIndex(
                name: "IX_Custos_DesenvolvimentoIdDesenvolvimento",
                table: "Custos");

            migrationBuilder.DropColumn(
                name: "DesenvolvimentoIdDesenvolvimento",
                table: "Custos");
        }
    }
}
