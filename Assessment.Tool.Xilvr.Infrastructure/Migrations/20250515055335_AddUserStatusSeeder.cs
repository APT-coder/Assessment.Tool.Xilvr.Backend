using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Assessment.Tool.Xilvr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserStatusSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "xilvr",
                table: "user_status",
                columns: new[] { "user_status_id", "active", "status" },
                values: new object[,]
                {
                    { (short)1, true, "Pending" },
                    { (short)2, true, "Active" },
                    { (short)3, true, "InActive" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "xilvr",
                table: "user_status",
                keyColumn: "user_status_id",
                keyValue: (short)1);

            migrationBuilder.DeleteData(
                schema: "xilvr",
                table: "user_status",
                keyColumn: "user_status_id",
                keyValue: (short)2);

            migrationBuilder.DeleteData(
                schema: "xilvr",
                table: "user_status",
                keyColumn: "user_status_id",
                keyValue: (short)3);
        }
    }
}
