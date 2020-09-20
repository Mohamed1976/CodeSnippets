using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLibrary.ContosoUniversity.Data.Migrations
{
    public partial class RenamedColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersonalInformation_EmailAddress",
                table: "Student",
                newName: "EmailAddress");

            migrationBuilder.RenameColumn(
                name: "PersonalInformation_DateOfBirth",
                table: "Student",
                newName: "DateOfBirth");

            migrationBuilder.RenameColumn(
                name: "PersonalInformation_EmailAddress",
                table: "Instructor",
                newName: "EmailAddress");

            migrationBuilder.RenameColumn(
                name: "PersonalInformation_DateOfBirth",
                table: "Instructor",
                newName: "DateOfBirth");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailAddress",
                table: "Student",
                newName: "PersonalInformation_EmailAddress");

            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "Student",
                newName: "PersonalInformation_DateOfBirth");

            migrationBuilder.RenameColumn(
                name: "EmailAddress",
                table: "Instructor",
                newName: "PersonalInformation_EmailAddress");

            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "Instructor",
                newName: "PersonalInformation_DateOfBirth");
        }
    }
}
