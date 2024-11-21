using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class Fixworkoutcomments5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<Guid>(
                name: "WorkoutId",
                table: "WorkoutCommentLikes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            _ = migrationBuilder.CreateIndex(
                name: "IX_WorkoutCommentLikes_WorkoutId",
                table: "WorkoutCommentLikes",
                column: "WorkoutId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_WorkoutCommentLikes_Workouts_WorkoutId",
                table: "WorkoutCommentLikes",
                column: "WorkoutId",
                principalTable: "Workouts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_WorkoutCommentLikes_Workouts_WorkoutId",
                table: "WorkoutCommentLikes");

            _ = migrationBuilder.DropIndex(
                name: "IX_WorkoutCommentLikes_WorkoutId",
                table: "WorkoutCommentLikes");

            _ = migrationBuilder.DropColumn(
                name: "WorkoutId",
                table: "WorkoutCommentLikes");
        }
    }
}
