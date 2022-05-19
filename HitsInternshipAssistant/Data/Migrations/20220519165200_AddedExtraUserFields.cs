using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HitsInternshipAssistant.Data.Migrations
{
    public partial class AddedExtraUserFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Contacts",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Patronymic",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ShowInApplicantsList",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowInInternsList",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contacts",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Patronymic",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ShowInApplicantsList",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ShowInInternsList",
                table: "AspNetUsers");
        }
    }
}
