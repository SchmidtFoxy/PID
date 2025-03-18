﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PID.Migrations
{
    /// <inheritdoc />
    public partial class CustoDesenvolvimentoIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Custo",
                table: "Desenvolvimentos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Custo",
                table: "Desenvolvimentos");
        }
    }
}
