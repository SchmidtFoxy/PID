using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PID.Migrations
{
    /// <inheritdoc />
    public partial class CorrigirTiposDeDados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Custos_Desenvolvimentos_IdDesenvolvimento",
                table: "Custos");

            migrationBuilder.DropForeignKey(
                name: "FK_Custos_Dispendios_IdDispendio",
                table: "Custos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjetosPD_Desenvolvimentos_IdDesenvolvimento",
                table: "ProjetosPD");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjetosPD_Dispendios_IdDispendio",
                table: "ProjetosPD");

            migrationBuilder.AlterColumn<int>(
                name: "Ano",
                table: "ProjetosPD",
                type: "int",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_Custos_Desenvolvimentos_IdDesenvolvimento",
                table: "Custos",
                column: "IdDesenvolvimento",
                principalTable: "Desenvolvimentos",
                principalColumn: "IdDesenvolvimento",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Custos_Dispendios_IdDispendio",
                table: "Custos",
                column: "IdDispendio",
                principalTable: "Dispendios",
                principalColumn: "IdDispendio",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetosPD_Desenvolvimentos_IdDesenvolvimento",
                table: "ProjetosPD",
                column: "IdDesenvolvimento",
                principalTable: "Desenvolvimentos",
                principalColumn: "IdDesenvolvimento",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetosPD_Dispendios_IdDispendio",
                table: "ProjetosPD",
                column: "IdDispendio",
                principalTable: "Dispendios",
                principalColumn: "IdDispendio",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Custos_Desenvolvimentos_IdDesenvolvimento",
                table: "Custos");

            migrationBuilder.DropForeignKey(
                name: "FK_Custos_Dispendios_IdDispendio",
                table: "Custos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjetosPD_Desenvolvimentos_IdDesenvolvimento",
                table: "ProjetosPD");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjetosPD_Dispendios_IdDispendio",
                table: "ProjetosPD");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Ano",
                table: "ProjetosPD",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Custos_Desenvolvimentos_IdDesenvolvimento",
                table: "Custos",
                column: "IdDesenvolvimento",
                principalTable: "Desenvolvimentos",
                principalColumn: "IdDesenvolvimento",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Custos_Dispendios_IdDispendio",
                table: "Custos",
                column: "IdDispendio",
                principalTable: "Dispendios",
                principalColumn: "IdDispendio",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetosPD_Desenvolvimentos_IdDesenvolvimento",
                table: "ProjetosPD",
                column: "IdDesenvolvimento",
                principalTable: "Desenvolvimentos",
                principalColumn: "IdDesenvolvimento",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetosPD_Dispendios_IdDispendio",
                table: "ProjetosPD",
                column: "IdDispendio",
                principalTable: "Dispendios",
                principalColumn: "IdDispendio",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
