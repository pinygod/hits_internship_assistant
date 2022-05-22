using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HitsInternshipAssistant.Data.Migrations
{
    public partial class PracticeJournalAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PracticeJournalId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PracticeJournal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Review = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeJournal", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PracticeJournalId",
                table: "AspNetUsers",
                column: "PracticeJournalId",
                unique: true,
                filter: "[PracticeJournalId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_PracticeJournal_PracticeJournalId",
                table: "AspNetUsers",
                column: "PracticeJournalId",
                principalTable: "PracticeJournal",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_PracticeJournal_PracticeJournalId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PracticeJournal");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PracticeJournalId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PracticeJournalId",
                table: "AspNetUsers");
        }
    }
}
