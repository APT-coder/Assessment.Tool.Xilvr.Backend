using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Assessment.Tool.Xilvr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "xilvr");

            migrationBuilder.CreateTable(
                name: "assessment",
                schema: "xilvr",
                columns: table => new
                {
                    assessment_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    assessment_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    total_marks = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assessment", x => x.assessment_id);
                });

            migrationBuilder.CreateTable(
                name: "batch",
                schema: "xilvr",
                columns: table => new
                {
                    batch_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    batch_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_batch", x => x.batch_id);
                });

            migrationBuilder.CreateTable(
                name: "permission_group",
                schema: "xilvr",
                columns: table => new
                {
                    permission_group_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    permission_group_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission_group", x => x.permission_group_id);
                });

            migrationBuilder.CreateTable(
                name: "question",
                schema: "xilvr",
                columns: table => new
                {
                    question_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    question_text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    question_type = table.Column<int>(type: "integer", nullable: false),
                    question_options = table.Column<List<string>>(type: "jsonb", nullable: true),
                    answer = table.Column<List<string>>(type: "jsonb", nullable: true),
                    question_points = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question", x => x.question_id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                schema: "xilvr",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    is_system_role = table.Column<bool>(type: "boolean", nullable: false),
                    is_default_role = table.Column<bool>(type: "boolean", nullable: false),
                    role_internal_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "user_status",
                schema: "xilvr",
                columns: table => new
                {
                    user_status_id = table.Column<short>(type: "smallint", nullable: false),
                    status = table.Column<string>(type: "character varying(15)", nullable: false),
                    active = table.Column<bool>(type: "boolean", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_status", x => x.user_status_id);
                });

            migrationBuilder.CreateTable(
                name: "scheduled_assessment",
                schema: "xilvr",
                columns: table => new
                {
                    scheduled_assessment_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    batch_id = table.Column<int>(type: "integer", nullable: false),
                    assessment_id = table.Column<int>(type: "integer", nullable: false),
                    assessment_duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    can_randomize = table.Column<bool>(type: "boolean", nullable: false),
                    can_display_result = table.Column<bool>(type: "boolean", nullable: false),
                    can_submit_before_end = table.Column<bool>(type: "boolean", nullable: false),
                    link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scheduled_assessment", x => x.scheduled_assessment_id);
                    table.ForeignKey(
                        name: "FK_scheduled_assessment_assessment_assessment_id",
                        column: x => x.assessment_id,
                        principalSchema: "xilvr",
                        principalTable: "assessment",
                        principalColumn: "assessment_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_scheduled_assessment_batch_batch_id",
                        column: x => x.batch_id,
                        principalSchema: "xilvr",
                        principalTable: "batch",
                        principalColumn: "batch_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "permission",
                schema: "xilvr",
                columns: table => new
                {
                    permission_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    permission_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    permission_key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    permission_group_id = table.Column<int>(type: "integer", nullable: true),
                    is_internal_permission = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission", x => x.permission_id);
                    table.ForeignKey(
                        name: "FK_permission_permission_group_permission_group_id",
                        column: x => x.permission_group_id,
                        principalSchema: "xilvr",
                        principalTable: "permission_group",
                        principalColumn: "permission_group_id");
                });

            migrationBuilder.CreateTable(
                name: "assessment_question",
                schema: "xilvr",
                columns: table => new
                {
                    assessment_id = table.Column<int>(type: "integer", nullable: false),
                    question_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assessment_question", x => new { x.assessment_id, x.question_id });
                    table.ForeignKey(
                        name: "FK_assessment_question_assessment_assessment_id",
                        column: x => x.assessment_id,
                        principalSchema: "xilvr",
                        principalTable: "assessment",
                        principalColumn: "assessment_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assessment_question_question_question_id",
                        column: x => x.question_id,
                        principalSchema: "xilvr",
                        principalTable: "question",
                        principalColumn: "question_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "xilvr",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uid = table.Column<Guid>(type: "uuid", nullable: false),
                    profile_image_url = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    user_status_id = table.Column<short>(type: "smallint", nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    password_reset_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_user_user_status_user_status_id",
                        column: x => x.user_status_id,
                        principalSchema: "xilvr",
                        principalTable: "user_status",
                        principalColumn: "user_status_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_permission",
                schema: "xilvr",
                columns: table => new
                {
                    role_permission_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    permission_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permission", x => x.role_permission_id);
                    table.ForeignKey(
                        name: "FK_role_permission_permission_permission_id",
                        column: x => x.permission_id,
                        principalSchema: "xilvr",
                        principalTable: "permission",
                        principalColumn: "permission_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_permission_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "xilvr",
                        principalTable: "role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee",
                schema: "xilvr",
                columns: table => new
                {
                    employee_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    batch_ids = table.Column<string>(type: "jsonb", nullable: true),
                    designation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee", x => x.employee_id);
                    table.ForeignKey(
                        name: "FK_employee_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "xilvr",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                schema: "xilvr",
                columns: table => new
                {
                    user_role_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_role", x => x.user_role_id);
                    table.ForeignKey(
                        name: "FK_user_role_role_role_id",
                        column: x => x.role_id,
                        principalSchema: "xilvr",
                        principalTable: "role",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_role_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "xilvr",
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "scheduled_assessment_answer",
                schema: "xilvr",
                columns: table => new
                {
                    scheduled_assessment_answer_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scheduled_assessment_id = table.Column<int>(type: "integer", nullable: false),
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    question_id = table.Column<int>(type: "integer", nullable: false),
                    answer = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    is_correct = table.Column<bool>(type: "boolean", nullable: false),
                    score = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scheduled_assessment_answer", x => x.scheduled_assessment_answer_id);
                    table.ForeignKey(
                        name: "FK_scheduled_assessment_answer_employee_employee_id",
                        column: x => x.employee_id,
                        principalSchema: "xilvr",
                        principalTable: "employee",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_scheduled_assessment_answer_question_question_id",
                        column: x => x.question_id,
                        principalSchema: "xilvr",
                        principalTable: "question",
                        principalColumn: "question_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_scheduled_assessment_answer_scheduled_assessment_scheduled_~",
                        column: x => x.scheduled_assessment_id,
                        principalSchema: "xilvr",
                        principalTable: "scheduled_assessment",
                        principalColumn: "scheduled_assessment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "scheduled_assessment_score",
                schema: "xilvr",
                columns: table => new
                {
                    scheduled_assessment_score_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scheduled_assessment_id = table.Column<int>(type: "integer", nullable: false),
                    employee_id = table.Column<long>(type: "bigint", nullable: false),
                    total_score = table.Column<double>(type: "double precision", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scheduled_assessment_score", x => x.scheduled_assessment_score_id);
                    table.ForeignKey(
                        name: "FK_scheduled_assessment_score_employee_employee_id",
                        column: x => x.employee_id,
                        principalSchema: "xilvr",
                        principalTable: "employee",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_scheduled_assessment_score_scheduled_assessment_scheduled_a~",
                        column: x => x.scheduled_assessment_id,
                        principalSchema: "xilvr",
                        principalTable: "scheduled_assessment",
                        principalColumn: "scheduled_assessment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_assessment_question_question_id",
                schema: "xilvr",
                table: "assessment_question",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_user_id",
                schema: "xilvr",
                table: "employee",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_permission_permission_group_id",
                schema: "xilvr",
                table: "permission",
                column: "permission_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_permission_id",
                schema: "xilvr",
                table: "role_permission",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_role_id",
                schema: "xilvr",
                table: "role_permission",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_scheduled_assessment_assessment_id",
                schema: "xilvr",
                table: "scheduled_assessment",
                column: "assessment_id");

            migrationBuilder.CreateIndex(
                name: "IX_scheduled_assessment_batch_id",
                schema: "xilvr",
                table: "scheduled_assessment",
                column: "batch_id");

            migrationBuilder.CreateIndex(
                name: "IX_scheduled_assessment_answer_employee_id",
                schema: "xilvr",
                table: "scheduled_assessment_answer",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_scheduled_assessment_answer_question_id",
                schema: "xilvr",
                table: "scheduled_assessment_answer",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_scheduled_assessment_answer_scheduled_assessment_id",
                schema: "xilvr",
                table: "scheduled_assessment_answer",
                column: "scheduled_assessment_id");

            migrationBuilder.CreateIndex(
                name: "IX_scheduled_assessment_score_employee_id",
                schema: "xilvr",
                table: "scheduled_assessment_score",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_scheduled_assessment_score_scheduled_assessment_id",
                schema: "xilvr",
                table: "scheduled_assessment_score",
                column: "scheduled_assessment_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_user_status_id",
                schema: "xilvr",
                table: "user",
                column: "user_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_role_id",
                schema: "xilvr",
                table: "user_role",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_user_id",
                schema: "xilvr",
                table: "user_role",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assessment_question",
                schema: "xilvr");

            migrationBuilder.DropTable(
                name: "role_permission",
                schema: "xilvr");

            migrationBuilder.DropTable(
                name: "scheduled_assessment_answer",
                schema: "xilvr");

            migrationBuilder.DropTable(
                name: "scheduled_assessment_score",
                schema: "xilvr");

            migrationBuilder.DropTable(
                name: "user_role",
                schema: "xilvr");

            migrationBuilder.DropTable(
                name: "permission",
                schema: "xilvr");

            migrationBuilder.DropTable(
                name: "question",
                schema: "xilvr");

            migrationBuilder.DropTable(
                name: "employee",
                schema: "xilvr");

            migrationBuilder.DropTable(
                name: "scheduled_assessment",
                schema: "xilvr");

            migrationBuilder.DropTable(
                name: "role",
                schema: "xilvr");

            migrationBuilder.DropTable(
                name: "permission_group",
                schema: "xilvr");

            migrationBuilder.DropTable(
                name: "user",
                schema: "xilvr");

            migrationBuilder.DropTable(
                name: "assessment",
                schema: "xilvr");

            migrationBuilder.DropTable(
                name: "batch",
                schema: "xilvr");

            migrationBuilder.DropTable(
                name: "user_status",
                schema: "xilvr");
        }
    }
}
