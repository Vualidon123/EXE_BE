using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EXE_BE.Migrations
{
    /// <inheritdoc />
    public partial class CreateListItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodItem");

            migrationBuilder.DropTable(
                name: "PlasticItem");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "PlasticUsages",
                newName: "ActivityId");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "FoodUsages",
                newName: "ActivityId");

            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "EnergyUsages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FoodItems",
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
                    table.PrimaryKey("PK_FoodItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodItems_FoodUsages_FoodUsageId",
                        column: x => x.FoodUsageId,
                        principalTable: "FoodUsages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlasticItems",
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
                    table.PrimaryKey("PK_PlasticItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlasticItems_PlasticUsages_PlasticUsageId",
                        column: x => x.PlasticUsageId,
                        principalTable: "PlasticUsages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodItems_FoodUsageId",
                table: "FoodItems",
                column: "FoodUsageId");

            migrationBuilder.CreateIndex(
                name: "IX_PlasticItems_PlasticUsageId",
                table: "PlasticItems",
                column: "PlasticUsageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodItems");

            migrationBuilder.DropTable(
                name: "PlasticItems");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "EnergyUsages");

            migrationBuilder.RenameColumn(
                name: "ActivityId",
                table: "PlasticUsages",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "ActivityId",
                table: "FoodUsages",
                newName: "userId");

            migrationBuilder.CreateTable(
                name: "FoodItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FoodCategory = table.Column<int>(type: "integer", nullable: false),
                    FoodUsageId = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false)
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
                    PlasticUsageId = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_FoodItem_FoodUsageId",
                table: "FoodItem",
                column: "FoodUsageId");

            migrationBuilder.CreateIndex(
                name: "IX_PlasticItem_PlasticUsageId",
                table: "PlasticItem",
                column: "PlasticUsageId");
        }
    }
}
