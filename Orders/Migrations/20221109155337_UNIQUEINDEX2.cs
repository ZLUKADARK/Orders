using Microsoft.EntityFrameworkCore.Migrations;

namespace Orders.Migrations
{
    public partial class UNIQUEINDEX2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Provider",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Ozon" },
                    { 2, "AliExpress" },
                    { 3, "Amazon" },
                    { 4, "Ebay" },
                    { 5, "Lite-Computer Store" },
                    { 6, "Cheap Wholesales" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Provider",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Provider",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Provider",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Provider",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Provider",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Provider",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
