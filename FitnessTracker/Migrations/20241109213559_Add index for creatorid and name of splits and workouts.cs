using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class Addindexforcreatoridandnameofsplitsandworkouts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Workouts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            _ = migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Splits",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Workouts_CreatorId_Name",
                table: "Workouts",
                columns: new[] { "CreatorId", "Name" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_Splits_CreatorId_Name",
                table: "Splits",
                columns: new[] { "CreatorId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropIndex(
                name: "IX_Workouts_CreatorId_Name",
                table: "Workouts");

            _ = migrationBuilder.DropIndex(
                name: "IX_Splits_CreatorId_Name",
                table: "Splits");

            _ = migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Workouts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            _ = migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Splits",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
