using Microsoft.EntityFrameworkCore.Migrations;

namespace HaragApp.Data.Migrations
{
    public partial class addShopSliderImg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "shopSliderImage",
                table: "Configs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "shopSliderImage",
                table: "Configs");
        }
    }
}
