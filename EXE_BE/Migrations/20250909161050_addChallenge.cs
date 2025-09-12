using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EXE_BE.Migrations
{
    /// <inheritdoc />
    public partial class addChallenge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Challenges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isComplete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Challenges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ChallengeId = table.Column<int>(type: "integer", nullable: false),
                    Progress = table.Column<float>(type: "real", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    FinishDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isComplete = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeProgresses_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeProgresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeProgresses_ChallengeId",
                table: "ChallengeProgresses",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeProgresses_UserId",
                table: "ChallengeProgresses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengeProgresses");

            migrationBuilder.DropTable(
                name: "Challenges");
        }
    }
}
