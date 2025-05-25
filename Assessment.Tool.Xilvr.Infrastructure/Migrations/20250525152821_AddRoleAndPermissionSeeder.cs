using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Assessment.Tool.Xilvr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleAndPermissionSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "xilvr",
                table: "permission_group",
                columns: new[] { "permission_group_id", "permission_group_name" },
                values: new object[,]
                {
                    { 1, "Trainee Listing - Trainee Listing" },
                    { 2, "Assessments - View Assessments Listing" },
                    { 3, "Assessments - Schedule Assessment" },
                    { 4, "Assessments - Question Bank" },
                    { 5, "Assessments - Administration" },
                    { 6, "Assessments - Reports" },
                    { 7, "Assessments - Evaluate Assessment" },
                    { 8, "Configuration Management - Manage User Roles" },
                    { 9, "Configuration Management - Manage Roles and Permissions" }
                });

            migrationBuilder.InsertData(
                schema: "xilvr",
                table: "role",
                columns: new[] { "role_id", "Description", "is_default_role", "is_system_role", "role_name", "role_internal_name" },
                values: new object[,]
                {
                    { 1, "Base Role", true, true, "Base Role", "BaseRole" },
                    { 2, "Trainee", false, true, "Trainee", "Trainee" },
                    { 3, "Trainer", false, true, "Trainer", "Trainer" },
                    { 4, "Trainer Manager", false, true, "Trainer Manager", "TrainerManager" },
                    { 5, "System Admin", false, true, "System Admin", "SystemAdmin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "xilvr",
                table: "permission_group",
                keyColumn: "permission_group_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "xilvr",
                table: "permission_group",
                keyColumn: "permission_group_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "xilvr",
                table: "permission_group",
                keyColumn: "permission_group_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "xilvr",
                table: "permission_group",
                keyColumn: "permission_group_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "xilvr",
                table: "permission_group",
                keyColumn: "permission_group_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "xilvr",
                table: "permission_group",
                keyColumn: "permission_group_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "xilvr",
                table: "permission_group",
                keyColumn: "permission_group_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "xilvr",
                table: "permission_group",
                keyColumn: "permission_group_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "xilvr",
                table: "permission_group",
                keyColumn: "permission_group_id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "xilvr",
                table: "role",
                keyColumn: "role_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "xilvr",
                table: "role",
                keyColumn: "role_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "xilvr",
                table: "role",
                keyColumn: "role_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "xilvr",
                table: "role",
                keyColumn: "role_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "xilvr",
                table: "role",
                keyColumn: "role_id",
                keyValue: 5);
        }
    }
}
