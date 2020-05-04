using Microsoft.EntityFrameworkCore.Migrations;

namespace Auth.Migrations
{
    public partial class InsertedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7d2b70d1-a25f-4714-916a-8a28401323a5", "f2d5a51f-10ad-471c-a4fd-13e224855095", "Visitor", "VISITOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "beadb7d8-eac9-4769-8a3e-8c74ca9ef923", "dd83bf65-d06b-4dca-81d9-bc636908aeb7", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7d2b70d1-a25f-4714-916a-8a28401323a5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "beadb7d8-eac9-4769-8a3e-8c74ca9ef923");
        }
    }
}
