using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PID.Migrations
{
    /// <inheritdoc />
    public partial class DetailDoDesenvolvimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoEdicoes_Desenvolvimentos_DesenvolvimentoId",
                table: "HistoricoEdicoes");

            migrationBuilder.RenameColumn(
                name: "ValorNovo",
                table: "HistoricoEdicoes",
                newName: "ValorAtual");

            migrationBuilder.RenameColumn(
                name: "DesenvolvimentoId",
                table: "HistoricoEdicoes",
                newName: "IdDesenvolvimento");

            migrationBuilder.RenameColumn(
                name: "DataEdicao",
                table: "HistoricoEdicoes",
                newName: "DataAlteracao");

            migrationBuilder.RenameIndex(
                name: "IX_HistoricoEdicoes_DesenvolvimentoId",
                table: "HistoricoEdicoes",
                newName: "IX_HistoricoEdicoes_IdDesenvolvimento");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoEdicoes_Desenvolvimentos_IdDesenvolvimento",
                table: "HistoricoEdicoes",
                column: "IdDesenvolvimento",
                principalTable: "Desenvolvimentos",
                principalColumn: "IdDesenvolvimento",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoEdicoes_Desenvolvimentos_IdDesenvolvimento",
                table: "HistoricoEdicoes");

            migrationBuilder.RenameColumn(
                name: "ValorAtual",
                table: "HistoricoEdicoes",
                newName: "ValorNovo");

            migrationBuilder.RenameColumn(
                name: "IdDesenvolvimento",
                table: "HistoricoEdicoes",
                newName: "DesenvolvimentoId");

            migrationBuilder.RenameColumn(
                name: "DataAlteracao",
                table: "HistoricoEdicoes",
                newName: "DataEdicao");

            migrationBuilder.RenameIndex(
                name: "IX_HistoricoEdicoes_IdDesenvolvimento",
                table: "HistoricoEdicoes",
                newName: "IX_HistoricoEdicoes_DesenvolvimentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoEdicoes_Desenvolvimentos_DesenvolvimentoId",
                table: "HistoricoEdicoes",
                column: "DesenvolvimentoId",
                principalTable: "Desenvolvimentos",
                principalColumn: "IdDesenvolvimento",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
