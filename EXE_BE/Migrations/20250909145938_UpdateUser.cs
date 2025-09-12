using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EXE_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_FoodUsages_FoodUsageId",
                table: "FoodItems");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionType",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_UserId",
                table: "UserActivities",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_FoodUsages_FoodUsageId",
                table: "FoodItems",
                column: "FoodUsageId",
                principalTable: "FoodUsages",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserActivities_Users_UserId",
                table: "UserActivities",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_FoodUsages_FoodUsageId",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserActivities_Users_UserId",
                table: "UserActivities");

            migrationBuilder.DropIndex(
                name: "IX_UserActivities_UserId",
                table: "UserActivities");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SubscriptionType",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_FoodUsages_FoodUsageId",
                table: "FoodItems",
                column: "FoodUsageId",
                principalTable: "FoodUsages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
