using Microsoft.EntityFrameworkCore.Migrations;

namespace HaragApp.Data.Migrations
{
    public partial class addedouterLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OutHyperlink1",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutHyperlink2",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutHyperlink3",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutlinkText1",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutlinkText2",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutlinkText3",
                table: "Configs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutHyperlink1",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "OutHyperlink2",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "OutHyperlink3",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "OutlinkText1",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "OutlinkText2",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "OutlinkText3",
                table: "Configs");
        }
    }
}
