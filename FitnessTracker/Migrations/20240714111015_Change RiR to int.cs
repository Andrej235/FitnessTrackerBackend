using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRiRtoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AlterColumn<int>(
                name: "RiR",
                table: "Sets",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.AlterColumn<float>(
                name: "RiR",
                table: "Sets",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
    }
}
