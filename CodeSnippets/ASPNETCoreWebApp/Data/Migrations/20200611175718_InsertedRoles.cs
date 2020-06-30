using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPNETCoreWebApp.Migrations.AppIdentityDb
{
    public partial class InsertedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "41022b77-ec2d-4364-ac61-99ef8abb95f0", "67a9b780-5ce8-495f-8af4-f58265b52512", "Visitor", "VISITOR" },
                    { "c9a5cb57-3118-4013-b124-9ae30bd0fbf9", "81ff61bc-7ac5-4132-b714-89c92fe39cc2", "Administrator", "ADMINISTRATOR" },
                    { "003fc384-be0f-449d-8a2d-93a260ca1c75", "8a327270-860e-48f2-87ef-52c7156547a0", "Director", "DIRECTOR" },
                    { "92187ddf-2216-4742-98a3-0eb8adea4c9d", "77af1e0a-876b-4aa3-8f5c-8913b69a7b7f", "Manager", "MANAGER" },
                    { "aea596b8-49d9-419b-9808-e21a6dedd6d3", "7ad41bde-ce3c-4dca-8055-e72ac685b0ba", "Employee", "EMPLOYEE" },
                    { "af4cb1d9-77be-4655-9ab8-ef405765714d", "b9e37922-820a-41e8-95f5-3c461d4e7bc8", "Guest", "GUEST" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "003fc384-be0f-449d-8a2d-93a260ca1c75");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41022b77-ec2d-4364-ac61-99ef8abb95f0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92187ddf-2216-4742-98a3-0eb8adea4c9d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aea596b8-49d9-419b-9808-e21a6dedd6d3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af4cb1d9-77be-4655-9ab8-ef405765714d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c9a5cb57-3118-4013-b124-9ae30bd0fbf9");
        }
    }
}
