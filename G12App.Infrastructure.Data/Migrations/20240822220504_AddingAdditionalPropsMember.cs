using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G12App.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingAdditionalPropsMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BaptismDate",
                table: "Members",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Baptized",
                table: "Members",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CPF",
                table: "Members",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "HasNoLeadership",
                table: "Members",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Idle",
                table: "Members",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Members",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Members_SpouseId",
                table: "Members",
                column: "SpouseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Members_SpouseId",
                table: "Members",
                column: "SpouseId",
                principalTable: "Members",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Members_SpouseId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_SpouseId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "BaptismDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Baptized",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "CPF",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "HasNoLeadership",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Idle",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Members");
        }
    }
}
