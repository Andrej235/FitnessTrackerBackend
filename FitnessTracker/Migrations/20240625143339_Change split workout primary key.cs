using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class Changesplitworkoutprimarykey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SplitWorkouts",
                table: "SplitWorkouts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SplitWorkouts",
                table: "SplitWorkouts",
                columns: new[] { "SplitId", "Day" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SplitWorkouts",
                table: "SplitWorkouts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SplitWorkouts",
                table: "SplitWorkouts",
                columns: new[] { "SplitId", "WorkoutId" });
        }
    }
}
