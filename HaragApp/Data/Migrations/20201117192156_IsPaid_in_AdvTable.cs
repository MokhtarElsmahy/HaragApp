using Microsoft.EntityFrameworkCore.Migrations;

namespace HaragApp.Data.Migrations
{
    public partial class IsPaid_in_AdvTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Advertisments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "IsPaidDescription",
                table: "Advertisments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Advertisments");

            migrationBuilder.DropColumn(
                name: "IsPaidDescription",
                table: "Advertisments");
        }
    }
}
