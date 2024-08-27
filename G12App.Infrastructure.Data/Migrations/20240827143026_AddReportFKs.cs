using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G12App.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReportFKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reports_CellId",
                table: "Reports",
                column: "CellId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Cells_CellId",
                table: "Reports",
                column: "CellId",
                principalTable: "Cells",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Cells_CellId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CellId",
                table: "Reports");
        }
    }
}
