using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class Addordertopins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "WorkoutPins",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "SplitPins",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "WorkoutPins");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "SplitPins");
        }
    }
}
