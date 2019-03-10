using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPersonalReviewer.Data.Migrations
{
    public partial class Smallchangetotheplacemodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LatLong",
                table: "Places",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LatLong",
                table: "Places");
        }
    }
}
