using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLibrary.BankDemo.Data.Migrations
{
    public partial class TypeAccountColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Accounts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Accounts");
        }
    }
}
