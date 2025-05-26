using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assessment.Tool.Xilvr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyUserEntityConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserProvider",
                schema: "xilvr",
                table: "user",
                newName: "user_provider");

            migrationBuilder.RenameColumn(
                name: "ProviderId",
                schema: "xilvr",
                table: "user",
                newName: "provider_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "user_provider",
                schema: "xilvr",
                table: "user",
                newName: "UserProvider");

            migrationBuilder.RenameColumn(
                name: "provider_id",
                schema: "xilvr",
                table: "user",
                newName: "ProviderId");
        }
    }
}
