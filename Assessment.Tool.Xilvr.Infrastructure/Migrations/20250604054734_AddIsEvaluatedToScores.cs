using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assessment.Tool.Xilvr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsEvaluatedToScores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_evaluated",
                schema: "xilvr",
                table: "scheduled_assessment_score",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_evaluated",
                schema: "xilvr",
                table: "scheduled_assessment_score");
        }
    }
}
