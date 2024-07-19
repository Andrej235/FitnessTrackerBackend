using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class Addcompletedsets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.RenameColumn(
                name: "NumberOfSets",
                table: "Sets",
                newName: "Type");

            _ = migrationBuilder.CreateTable(
                name: "CompletedSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompletedWorkoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_CompletedSets", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_CompletedSets_CompletedWorkouts_CompletedWorkoutId",
                        column: x => x.CompletedWorkoutId,
                        principalTable: "CompletedWorkouts",
                        principalColumn: "Id");
                    _ = table.ForeignKey(
                        name: "FK_CompletedSets_Sets_SetId",
                        column: x => x.SetId,
                        principalTable: "Sets",
                        principalColumn: "Id");
                    _ = table.ForeignKey(
                        name: "FK_CompletedSets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_CompletedSets_CompletedWorkoutId",
                table: "CompletedSets",
                column: "CompletedWorkoutId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_CompletedSets_SetId",
                table: "CompletedSets",
                column: "SetId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_CompletedSets_UserId",
                table: "CompletedSets",
                column: "UserId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_CompletedSets_UserId_SetId",
                table: "CompletedSets",
                columns: new[] { "UserId", "SetId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "CompletedSets");

            _ = migrationBuilder.RenameColumn(
                name: "Type",
                table: "Sets",
                newName: "NumberOfSets");
        }
    }
}
