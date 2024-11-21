using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class RemoveisPublicfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropIndex(
                name: "IX_Workouts_IsPublic",
                table: "Workouts");

            _ = migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Workouts");

            _ = migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Splits");

            _ = migrationBuilder.AddColumn<DateTime>(
                name: "LikedAt",
                table: "SplitLikes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.AddColumn<DateTime>(
                name: "FavoritedAt",
                table: "FavoriteSplits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "LikedAt",
                table: "SplitLikes");

            _ = migrationBuilder.DropColumn(
                name: "FavoritedAt",
                table: "FavoriteSplits");

            _ = migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Workouts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Splits",
                type: "bit",
                nullable: false,
                defaultValue: false);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Workouts_IsPublic",
                table: "Workouts",
                column: "IsPublic");
        }
    }
}
