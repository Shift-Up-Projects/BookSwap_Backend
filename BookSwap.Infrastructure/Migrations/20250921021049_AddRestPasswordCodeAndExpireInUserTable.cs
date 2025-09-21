using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookSwap.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRestPasswordCodeAndExpireInUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "AspNetUsers",
                newName: "ResetPasswordCode");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordCodeExpiry",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordCodeExpiry",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ResetPasswordCode",
                table: "AspNetUsers",
                newName: "Code");
        }
    }
}
