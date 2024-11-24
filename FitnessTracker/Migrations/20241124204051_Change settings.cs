using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class Changesettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PublicCompletedWorkouts",
                table: "UserSettings",
                newName: "PublicCreatedWorkouts");

            migrationBuilder.AddColumn<bool>(
                name: "PublicCreatedSplits",
                table: "UserSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicCreatedSplits",
                table: "UserSettings");

            migrationBuilder.RenameColumn(
                name: "PublicCreatedWorkouts",
                table: "UserSettings",
                newName: "PublicCompletedWorkouts");
        }
    }
}
