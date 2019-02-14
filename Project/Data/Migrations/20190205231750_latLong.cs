using Microsoft.EntityFrameworkCore.Migrations;

namespace MyPersonalReviewer.Data.Migrations
{
    public partial class latLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Lat",
                table: "Places",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Long",
                table: "Places",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "Long",
                table: "Places");
        }
    }
}
