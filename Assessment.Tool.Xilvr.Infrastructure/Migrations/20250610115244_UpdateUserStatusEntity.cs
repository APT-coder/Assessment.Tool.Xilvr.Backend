using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assessment.Tool.Xilvr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserStatusEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "xilvr",
                table: "user_status",
                type: "character varying(40)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(15)");

            migrationBuilder.UpdateData(
                schema: "xilvr",
                table: "user_status",
                keyColumn: "user_status_id",
                keyValue: (short)1,
                column: "status",
                value: "PendingProfileCompletion");

            migrationBuilder.UpdateData(
                schema: "xilvr",
                table: "user_status",
                keyColumn: "user_status_id",
                keyValue: (short)2,
                column: "status",
                value: "PendingApproval");

            migrationBuilder.UpdateData(
                schema: "xilvr",
                table: "user_status",
                keyColumn: "user_status_id",
                keyValue: (short)3,
                column: "status",
                value: "Active");

            migrationBuilder.InsertData(
                schema: "xilvr",
                table: "user_status",
                columns: new[] { "user_status_id", "active", "status" },
                values: new object[] { (short)4, true, "InActive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "xilvr",
                table: "user_status",
                keyColumn: "user_status_id",
                keyValue: (short)4);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "xilvr",
                table: "user_status",
                type: "character varying(15)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(40)");

            migrationBuilder.UpdateData(
                schema: "xilvr",
                table: "user_status",
                keyColumn: "user_status_id",
                keyValue: (short)1,
                column: "status",
                value: "Pending");

            migrationBuilder.UpdateData(
                schema: "xilvr",
                table: "user_status",
                keyColumn: "user_status_id",
                keyValue: (short)2,
                column: "status",
                value: "Active");

            migrationBuilder.UpdateData(
                schema: "xilvr",
                table: "user_status",
                keyColumn: "user_status_id",
                keyValue: (short)3,
                column: "status",
                value: "InActive");
        }
    }
}
