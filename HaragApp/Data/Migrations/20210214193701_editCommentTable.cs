using Microsoft.EntityFrameworkCore.Migrations;

namespace HaragApp.Data.Migrations
{
    public partial class editCommentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Advertisments_AdvertismentAdID",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_AdvertismentAdID",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "AdvertismentAdID",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "advID",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_advID",
                table: "Comments",
                column: "advID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Advertisments_advID",
                table: "Comments",
                column: "advID",
                principalTable: "Advertisments",
                principalColumn: "AdID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Advertisments_advID",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_advID",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "advID",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "AdvertismentAdID",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AdvertismentAdID",
                table: "Comments",
                column: "AdvertismentAdID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Advertisments_AdvertismentAdID",
                table: "Comments",
                column: "AdvertismentAdID",
                principalTable: "Advertisments",
                principalColumn: "AdID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
