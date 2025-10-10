using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EXE_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateActivity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_FoodUsages_FoodUsageId",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserActivities_EnergyUsages_EnergyUsageId",
                table: "UserActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_UserActivities_FoodUsages_FoodUsageId",
                table: "UserActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_UserActivities_PlasticUsages_PlasticUsageId",
                table: "UserActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_UserActivities_TrafficUsages_TrafficUsageId",
                table: "UserActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_UserActivities_Users_UserId",
                table: "UserActivities");

            migrationBuilder.DropIndex(
                name: "IX_Users_Id",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserActivities_EnergyUsageId",
                table: "UserActivities");

            migrationBuilder.DropIndex(
                name: "IX_UserActivities_FoodUsageId",
                table: "UserActivities");

            migrationBuilder.DropIndex(
                name: "IX_UserActivities_PlasticUsageId",
                table: "UserActivities");

            migrationBuilder.DropIndex(
                name: "IX_UserActivities_TrafficUsageId",
                table: "UserActivities");

            migrationBuilder.AlterColumn<float>(
                name: "TotalCO2Emission",
                table: "UserActivities",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "UserActivities",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "distance",
                table: "TrafficUsages",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "CO2emission",
                table: "TrafficUsages",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "CO2emission",
                table: "PlasticUsages",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "Weight",
                table: "PlasticItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "CO2emission",
                table: "FoodUsages",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "Weight",
                table: "FoodItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "electricityconsumption",
                table: "EnergyUsages",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<float>(
                name: "CO2emission",
                table: "EnergyUsages",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_UserId1",
                table: "UserActivities",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_TrafficUsages_ActivityId",
                table: "TrafficUsages",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlasticUsages_ActivityId",
                table: "PlasticUsages",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoodUsages_ActivityId",
                table: "FoodUsages",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnergyUsages_ActivityId",
                table: "EnergyUsages",
                column: "ActivityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EnergyUsages_UserActivities_ActivityId",
                table: "EnergyUsages",
                column: "ActivityId",
                principalTable: "UserActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_FoodUsages_FoodUsageId",
                table: "FoodItems",
                column: "FoodUsageId",
                principalTable: "FoodUsages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodUsages_UserActivities_ActivityId",
                table: "FoodUsages",
                column: "ActivityId",
                principalTable: "UserActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlasticUsages_UserActivities_ActivityId",
                table: "PlasticUsages",
                column: "ActivityId",
                principalTable: "UserActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrafficUsages_UserActivities_ActivityId",
                table: "TrafficUsages",
                column: "ActivityId",
                principalTable: "UserActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserActivities_Users_UserId",
                table: "UserActivities",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserActivities_Users_UserId1",
                table: "UserActivities",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnergyUsages_UserActivities_ActivityId",
                table: "EnergyUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_FoodUsages_FoodUsageId",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodUsages_UserActivities_ActivityId",
                table: "FoodUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_PlasticUsages_UserActivities_ActivityId",
                table: "PlasticUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_TrafficUsages_UserActivities_ActivityId",
                table: "TrafficUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserActivities_Users_UserId",
                table: "UserActivities");

            migrationBuilder.DropForeignKey(
                name: "FK_UserActivities_Users_UserId1",
                table: "UserActivities");

            migrationBuilder.DropIndex(
                name: "IX_UserActivities_UserId1",
                table: "UserActivities");

            migrationBuilder.DropIndex(
                name: "IX_TrafficUsages_ActivityId",
                table: "TrafficUsages");

            migrationBuilder.DropIndex(
                name: "IX_PlasticUsages_ActivityId",
                table: "PlasticUsages");

            migrationBuilder.DropIndex(
                name: "IX_FoodUsages_ActivityId",
                table: "FoodUsages");

            migrationBuilder.DropIndex(
                name: "IX_EnergyUsages_ActivityId",
                table: "EnergyUsages");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserActivities");

            migrationBuilder.AlterColumn<float>(
                name: "TotalCO2Emission",
                table: "UserActivities",
                type: "real",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "distance",
                table: "TrafficUsages",
                type: "real",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "CO2emission",
                table: "TrafficUsages",
                type: "real",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "CO2emission",
                table: "PlasticUsages",
                type: "real",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "Weight",
                table: "PlasticItems",
                type: "real",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "CO2emission",
                table: "FoodUsages",
                type: "real",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "Weight",
                table: "FoodItems",
                type: "real",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "electricityconsumption",
                table: "EnergyUsages",
                type: "real",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "CO2emission",
                table: "EnergyUsages",
                type: "real",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                table: "Users",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_EnergyUsageId",
                table: "UserActivities",
                column: "EnergyUsageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_FoodUsageId",
                table: "UserActivities",
                column: "FoodUsageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_PlasticUsageId",
                table: "UserActivities",
                column: "PlasticUsageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_TrafficUsageId",
                table: "UserActivities",
                column: "TrafficUsageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_FoodUsages_FoodUsageId",
                table: "FoodItems",
                column: "FoodUsageId",
                principalTable: "FoodUsages",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserActivities_EnergyUsages_EnergyUsageId",
                table: "UserActivities",
                column: "EnergyUsageId",
                principalTable: "EnergyUsages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserActivities_FoodUsages_FoodUsageId",
                table: "UserActivities",
                column: "FoodUsageId",
                principalTable: "FoodUsages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserActivities_PlasticUsages_PlasticUsageId",
                table: "UserActivities",
                column: "PlasticUsageId",
                principalTable: "PlasticUsages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserActivities_TrafficUsages_TrafficUsageId",
                table: "UserActivities",
                column: "TrafficUsageId",
                principalTable: "TrafficUsages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserActivities_Users_UserId",
                table: "UserActivities",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
