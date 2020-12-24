using Microsoft.EntityFrameworkCore.Migrations;

namespace HaragApp.Data.Migrations
{
    public partial class Add_Lag_Lat_in_City : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Langtude",
                table: "Cities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lantitude",
                table: "Cities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Langtude",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "Lantitude",
                table: "Cities");
        }
    }
}
