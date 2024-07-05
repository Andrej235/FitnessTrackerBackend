using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class Fixcompletedset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "CompletedSets");

            migrationBuilder.AddColumn<int>(
                name: "RepsCompleted",
                table: "CompletedSets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "WeightUsed",
                table: "CompletedSets",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepsCompleted",
                table: "CompletedSets");

            migrationBuilder.DropColumn(
                name: "WeightUsed",
                table: "CompletedSets");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "CompletedSets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
