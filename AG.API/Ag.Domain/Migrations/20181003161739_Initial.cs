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
                    Role = table.Column<int>(nullable: false),
                    Shift = table.Column<int>(nullable: false),
                    ColleagueId = table.Column<int>(nullable: true),
                    Sites = table.Column<string>(nullable: true)
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
                    SiteName = table.Column<string>(nullable: false),
                    IncomeInDollars = table.Column<double>(nullable: false),
                    WorkDay = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomeEntries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomeEntries_UserId",
                table: "IncomeEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ColleagueId",
                table: "Users",
                column: "ColleagueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomeEntries");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
