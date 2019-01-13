using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPersonalReviewer.Data.Migrations
{
    public partial class chageToMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorsId",
                table: "Menus",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PlaceId",
                table: "Menus",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorsId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "Menus");
        }
    }
}
