using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HitsInternshipAssistant.Data.Migrations
{
    public partial class StudentReviewAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Review = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentReviews_AspNetUsers_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentReviews_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentReviews_ReviewerId",
                table: "StudentReviews",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentReviews_StudentId",
                table: "StudentReviews",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentReviews");
        }
    }
}
