using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HitsInternshipAssistant.Data.Migrations
{
    public partial class WorkDirectionsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkDirections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkDirections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentWorkDirections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentDirectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Experience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stack = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentWorkDirections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentWorkDirections_WorkDirections_ParentDirectionId",
                        column: x => x.ParentDirectionId,
                        principalTable: "WorkDirections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentWorkDirections_ParentDirectionId",
                table: "StudentWorkDirections",
                column: "ParentDirectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentWorkDirections");

            migrationBuilder.DropTable(
                name: "WorkDirections");
        }
    }
}
