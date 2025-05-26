using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assessment.Tool.Xilvr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHashToQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "content_hash",
                schema: "xilvr",
                table: "question",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "content_hash",
                schema: "xilvr",
                table: "question");
        }
    }
}
