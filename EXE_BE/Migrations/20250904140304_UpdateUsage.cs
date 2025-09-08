using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EXE_BE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EnergyUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    electricityconsumption = table.Column<float>(type: "real", nullable: false),
                    CO2emission = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyUsages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userId = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CO2emission = table.Column<float>(type: "real", nullable: false),
                    score = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodUsages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlasticUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userId = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CO2emission = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlasticUsages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrafficUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActivityId = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    distance = table.Column<float>(type: "real", nullable: false),
                    trafficCategory = table.Column<int>(type: "integer", nullable: false),
                    CO2emission = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficUsages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FoodCategory = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    FoodUsageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodItem_FoodUsages_FoodUsageId",
                        column: x => x.FoodUsageId,
                        principalTable: "FoodUsages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlasticItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlasticCategory = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    PlasticUsageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlasticItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlasticItem_PlasticUsages_PlasticUsageId",
                        column: x => x.PlasticUsageId,
                        principalTable: "PlasticUsages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    PlasticUsageId = table.Column<int>(type: "integer", nullable: false),
                    TrafficUsageId = table.Column<int>(type: "integer", nullable: false),
                    FoodUsageId = table.Column<int>(type: "integer", nullable: false),
                    EnergyUsageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserActivities_EnergyUsages_EnergyUsageId",
                        column: x => x.EnergyUsageId,
                        principalTable: "EnergyUsages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserActivities_FoodUsages_FoodUsageId",
                        column: x => x.FoodUsageId,
                        principalTable: "FoodUsages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserActivities_PlasticUsages_PlasticUsageId",
                        column: x => x.PlasticUsageId,
                        principalTable: "PlasticUsages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserActivities_TrafficUsages_TrafficUsageId",
                        column: x => x.TrafficUsageId,
                        principalTable: "TrafficUsages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodItem_FoodUsageId",
                table: "FoodItem",
                column: "FoodUsageId");

            migrationBuilder.CreateIndex(
                name: "IX_PlasticItem_PlasticUsageId",
                table: "PlasticItem",
                column: "PlasticUsageId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodItem");

            migrationBuilder.DropTable(
                name: "PlasticItem");

            migrationBuilder.DropTable(
                name: "UserActivities");

            migrationBuilder.DropTable(
                name: "EnergyUsages");

            migrationBuilder.DropTable(
                name: "FoodUsages");

            migrationBuilder.DropTable(
                name: "PlasticUsages");

            migrationBuilder.DropTable(
                name: "TrafficUsages");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");
        }
    }
}
