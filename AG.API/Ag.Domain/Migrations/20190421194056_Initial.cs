using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Ag.Domain.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserName = table.Column<string>(nullable: false),
                    PasswordHash = table.Column<byte[]>(nullable: false),
                    PasswordSalt = table.Column<byte[]>(nullable: false),
                    MinPercent = table.Column<double>(nullable: false),
                    MaxPercent = table.Column<double>(nullable: false),
                    LastPercent = table.Column<double>(nullable: false),
                    Role = table.Column<int>(nullable: false),
                    Shift = table.Column<int>(nullable: false),
                    ColleagueId = table.Column<int>(nullable: true),
                    Sites = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_ColleagueId",
                        column: x => x.ColleagueId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomeEntries",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Locked = table.Column<bool>(nullable: false),
                    TotalSum = table.Column<double>(nullable: false),
                    TotalIncomeForStudio = table.Column<double>(nullable: false),
                    TotalIncomeForOperator = table.Column<double>(nullable: false),
                    TotalIncomeForPerformer = table.Column<double>(nullable: false),
                    CurrentOperatorPercent = table.Column<double>(nullable: false),
                    CurrentPerformerPercent = table.Column<double>(nullable: false),
                    AboveAverageThreshold = table.Column<bool>(nullable: false),
                    OperatorId = table.Column<int>(nullable: false),
                    PerformerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomeEntries_Users_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncomeEntries_Users_PerformerId",
                        column: x => x.PerformerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRelations",
                columns: table => new
                {
                    FromId = table.Column<int>(nullable: false),
                    ToId = table.Column<int>(nullable: false),
                    Color = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRelations", x => new { x.FromId, x.ToId });
                    table.ForeignKey(
                        name: "FK_UserRelations_Users_FromId",
                        column: x => x.FromId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRelations_Users_ToId",
                        column: x => x.ToId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkDays",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Shift = table.Column<int>(nullable: false),
                    OperatorId = table.Column<int>(nullable: false),
                    PerformerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkDays_Users_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkDays_Users_PerformerId",
                        column: x => x.PerformerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncomeChunks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Site = table.Column<int>(nullable: false),
                    Sum = table.Column<double>(nullable: false),
                    IncomeForStudio = table.Column<double>(nullable: false),
                    IncomeForOperator = table.Column<double>(nullable: false),
                    IncomeForPerformer = table.Column<double>(nullable: false),
                    IncomeEntryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeChunks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomeChunks_IncomeEntries_IncomeEntryId",
                        column: x => x.IncomeEntryId,
                        principalTable: "IncomeEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomeChunks_IncomeEntryId",
                table: "IncomeChunks",
                column: "IncomeEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeEntries_OperatorId",
                table: "IncomeEntries",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeEntries_PerformerId",
                table: "IncomeEntries",
                column: "PerformerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRelations_ToId",
                table: "UserRelations",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ColleagueId",
                table: "Users",
                column: "ColleagueId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkDays_OperatorId",
                table: "WorkDays",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkDays_PerformerId",
                table: "WorkDays",
                column: "PerformerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomeChunks");

            migrationBuilder.DropTable(
                name: "UserRelations");

            migrationBuilder.DropTable(
                name: "WorkDays");

            migrationBuilder.DropTable(
                name: "IncomeEntries");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
