using Microsoft.EntityFrameworkCore.Migrations;

namespace HaragApp.Data.Migrations
{
    public partial class editconfigs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaceBookLink",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mobile1",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mobile2",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OurHistory",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OurMessage",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterLink",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkFrom",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkTo",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "slider1Image",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "slider1Text",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "slider2Image",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "slider2Text",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "slider3Image",
                table: "Configs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "slider3Text",
                table: "Configs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "FaceBookLink",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "Mobile1",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "Mobile2",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "OurHistory",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "OurMessage",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "TwitterLink",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "WorkFrom",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "WorkTo",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "address",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "slider1Image",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "slider1Text",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "slider2Image",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "slider2Text",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "slider3Image",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "slider3Text",
                table: "Configs");
        }
    }
}
