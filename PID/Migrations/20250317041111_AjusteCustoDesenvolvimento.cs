using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PID.Migrations
{
    /// <inheritdoc />
    public partial class AjusteCustoDesenvolvimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TempoDesenvolvimento",
                table: "Desenvolvimentos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TempoDesenvolvimento",
                table: "Desenvolvimentos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
