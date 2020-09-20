using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLibrary.ContosoUniversity.Data.Migrations
{
    public partial class AddedEmailAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PersonalInformation_DateOfBirth",
                table: "Student",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalInformation_EmailAddress",
                table: "Student",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PersonalInformation_DateOfBirth",
                table: "Instructor",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonalInformation_EmailAddress",
                table: "Instructor",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalInformation_DateOfBirth",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "PersonalInformation_EmailAddress",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "PersonalInformation_DateOfBirth",
                table: "Instructor");

            migrationBuilder.DropColumn(
                name: "PersonalInformation_EmailAddress",
                table: "Instructor");
        }
    }
}
