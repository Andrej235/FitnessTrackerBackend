using System;
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
            migrationBuilder.RenameColumn(
                name: "NumberOfSets",
                table: "Sets",
                newName: "Type");

            migrationBuilder.CreateTable(
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
                    table.PrimaryKey("PK_CompletedSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedSets_CompletedWorkouts_CompletedWorkoutId",
                        column: x => x.CompletedWorkoutId,
                        principalTable: "CompletedWorkouts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompletedSets_Sets_SetId",
                        column: x => x.SetId,
                        principalTable: "Sets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompletedSets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedSets_CompletedWorkoutId",
                table: "CompletedSets",
                column: "CompletedWorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedSets_SetId",
                table: "CompletedSets",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedSets_UserId",
                table: "CompletedSets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedSets_UserId_SetId",
                table: "CompletedSets",
                columns: new[] { "UserId", "SetId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedSets");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Sets",
                newName: "NumberOfSets");
        }
    }
}
