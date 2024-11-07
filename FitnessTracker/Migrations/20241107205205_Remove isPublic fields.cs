using System;
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
            migrationBuilder.DropIndex(
                name: "IX_Workouts_IsPublic",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Splits");

            migrationBuilder.AddColumn<DateTime>(
                name: "LikedAt",
                table: "SplitLikes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FavoritedAt",
                table: "FavoriteSplits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikedAt",
                table: "SplitLikes");

            migrationBuilder.DropColumn(
                name: "FavoritedAt",
                table: "FavoriteSplits");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Workouts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Splits",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_IsPublic",
                table: "Workouts",
                column: "IsPublic");
        }
    }
}
