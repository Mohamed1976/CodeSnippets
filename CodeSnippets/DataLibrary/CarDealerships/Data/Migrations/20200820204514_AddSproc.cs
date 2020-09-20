using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLibrary.CarDealerships.Data.Migrations
{
    public partial class AddSproc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            MigrationHelpers.CreateView(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            MigrationHelpers.DropView(migrationBuilder);
        }
    }
}
