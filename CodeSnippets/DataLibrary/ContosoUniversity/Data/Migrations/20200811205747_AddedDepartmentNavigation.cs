using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLibrary.ContosoUniversity.Data.Migrations
{
    public partial class AddedDepartmentNavigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersonalInformation_LastName",
                table: "Student",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "PersonalInformation_LastName",
                table: "Instructor",
                newName: "LastName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Student",
                newName: "PersonalInformation_LastName");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Instructor",
                newName: "PersonalInformation_LastName");
        }
    }
}
