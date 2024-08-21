using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G12App.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Report_WeekCellSkipped : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WeekCellSkipped",
                table: "Reports",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeekCellSkipped",
                table: "Reports");
        }
    }
}
