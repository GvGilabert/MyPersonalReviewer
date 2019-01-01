using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPersonalReviewer.Data.Migrations
{
    public partial class ChangesToReviewsDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Reviews",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PlaceId",
                table: "Reviews",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Reviews",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Reviews");
        }
    }
}
