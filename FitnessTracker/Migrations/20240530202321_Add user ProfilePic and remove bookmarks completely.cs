using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class AdduserProfilePicandremovebookmarkscompletely : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseBookmarks");

            migrationBuilder.AddColumn<int>(
                name: "ExerciseId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePic",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ExerciseId",
                table: "Users",
                column: "ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Exercises_ExerciseId",
                table: "Users",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Exercises_ExerciseId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ExerciseId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ExerciseId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfilePic",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "ExerciseBookmarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseBookmarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseBookmarks_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseBookmarks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseBookmarks_ExerciseId",
                table: "ExerciseBookmarks",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseBookmarks_UserId",
                table: "ExerciseBookmarks",
                column: "UserId");
        }
    }
}
