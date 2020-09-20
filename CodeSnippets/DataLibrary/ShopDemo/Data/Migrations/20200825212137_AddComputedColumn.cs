using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLibrary.ShopDemo.Data.Migrations
{
    public partial class AddComputedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OrderTotal",
                schema: "Store",
                table: "Orders",
                type: "money",
                nullable: true,
                computedColumnSql: "Store.GetOrderTotal([Id])");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderTotal",
                schema: "Store",
                table: "Orders");
        }
    }
}
