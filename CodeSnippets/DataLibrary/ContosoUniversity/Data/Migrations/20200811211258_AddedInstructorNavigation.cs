using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLibrary.ContosoUniversity.Data.Migrations
{
    public partial class AddedInstructorNavigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OfficeAssignment",
                table: "OfficeAssignment");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OfficeAssignment",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<byte[]>(
                name: "TimeStamp",
                table: "OfficeAssignment",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfficeAssignment",
                table: "OfficeAssignment",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeAssignment_InstructorID",
                table: "OfficeAssignment",
                column: "InstructorID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OfficeAssignment",
                table: "OfficeAssignment");

            migrationBuilder.DropIndex(
                name: "IX_OfficeAssignment_InstructorID",
                table: "OfficeAssignment");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OfficeAssignment");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "OfficeAssignment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfficeAssignment",
                table: "OfficeAssignment",
                column: "InstructorID");
        }
    }
}
