using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EXE_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChallenge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "UserActivities");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "ChallengeProgresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "ChallengeProgresses");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "UserActivities",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
