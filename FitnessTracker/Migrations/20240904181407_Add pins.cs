using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class Addpins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "SplitPins",
                columns: table => new
                {
                    SplitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_SplitPins", x => new { x.UserId, x.SplitId });
                    _ = table.ForeignKey(
                        name: "FK_SplitPins_Splits_SplitId",
                        column: x => x.SplitId,
                        principalTable: "Splits",
                        principalColumn: "Id");
                    _ = table.ForeignKey(
                        name: "FK_SplitPins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateTable(
                name: "WorkoutPins",
                columns: table => new
                {
                    WorkoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_WorkoutPins", x => new { x.UserId, x.WorkoutId });
                    _ = table.ForeignKey(
                        name: "FK_WorkoutPins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    _ = table.ForeignKey(
                        name: "FK_WorkoutPins_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_SplitPins_SplitId",
                table: "SplitPins",
                column: "SplitId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_SplitPins_UserId_SplitId",
                table: "SplitPins",
                columns: new[] { "UserId", "SplitId" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_WorkoutPins_UserId_WorkoutId",
                table: "WorkoutPins",
                columns: new[] { "UserId", "WorkoutId" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_WorkoutPins_WorkoutId",
                table: "WorkoutPins",
                column: "WorkoutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "SplitPins");

            _ = migrationBuilder.DropTable(
                name: "WorkoutPins");
        }
    }
}
