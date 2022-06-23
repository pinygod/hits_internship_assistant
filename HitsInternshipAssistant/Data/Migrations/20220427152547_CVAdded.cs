using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HitsInternshipAssistant.Data.Migrations
{
    public partial class CVAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CVId",
                table: "StudentWorkDirections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CVId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CVs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contacts = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentWorkDirections_CVId",
                table: "StudentWorkDirections",
                column: "CVId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CVId",
                table: "AspNetUsers",
                column: "CVId",
                unique: true,
                filter: "[CVId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CVs_CVId",
                table: "AspNetUsers",
                column: "CVId",
                principalTable: "CVs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentWorkDirections_CVs_CVId",
                table: "StudentWorkDirections",
                column: "CVId",
                principalTable: "CVs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CVs_CVId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentWorkDirections_CVs_CVId",
                table: "StudentWorkDirections");

            migrationBuilder.DropTable(
                name: "CVs");

            migrationBuilder.DropIndex(
                name: "IX_StudentWorkDirections_CVId",
                table: "StudentWorkDirections");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CVId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CVId",
                table: "StudentWorkDirections");

            migrationBuilder.DropColumn(
                name: "CVId",
                table: "AspNetUsers");
        }
    }
}
