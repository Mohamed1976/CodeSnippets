using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLibrary.ContosoUniversity.Data.Migrations
{
    public partial class IsDeletedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Dbo",
                table: "Course",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Student",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OfficeAssignment",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Instructor",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Enrollment",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Department",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Dbo",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OfficeAssignment");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Instructor");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Enrollment");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Department");
        }
    }
}
