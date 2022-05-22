using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HitsInternshipAssistant.Data.Migrations
{
    public partial class VacancyUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Vacancies",
                newName: "TechStack");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInfo",
                table: "Vacancies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RequiredSkills",
                table: "Vacancies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInfo",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "RequiredSkills",
                table: "Vacancies");

            migrationBuilder.RenameColumn(
                name: "TechStack",
                table: "Vacancies",
                newName: "Description");
        }
    }
}
