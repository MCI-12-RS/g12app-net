using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G12App.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberG12Id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComputedG12LeaderId",
                table: "Members",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComputedG12LeaderId",
                table: "Members");
        }
    }
}
