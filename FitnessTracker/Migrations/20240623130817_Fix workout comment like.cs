using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class Fixworkoutcommentlike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutCommentLikes_WorkoutComments_CommentId",
                table: "WorkoutCommentLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutCommentLikes_WorkoutComments_WorkoutCommentId",
                table: "WorkoutCommentLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutCommentLikes",
                table: "WorkoutCommentLikes");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutCommentLikes_CommentId",
                table: "WorkoutCommentLikes");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutCommentLikes_CommentId_UserId",
                table: "WorkoutCommentLikes");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "WorkoutCommentLikes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutCommentLikes",
                table: "WorkoutCommentLikes",
                columns: new[] { "WorkoutCommentId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutCommentLikes_WorkoutCommentId_UserId",
                table: "WorkoutCommentLikes",
                columns: new[] { "WorkoutCommentId", "UserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutCommentLikes_WorkoutComments_WorkoutCommentId",
                table: "WorkoutCommentLikes",
                column: "WorkoutCommentId",
                principalTable: "WorkoutComments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutCommentLikes_WorkoutComments_WorkoutCommentId",
                table: "WorkoutCommentLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkoutCommentLikes",
                table: "WorkoutCommentLikes");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutCommentLikes_WorkoutCommentId_UserId",
                table: "WorkoutCommentLikes");

            migrationBuilder.AddColumn<Guid>(
                name: "CommentId",
                table: "WorkoutCommentLikes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkoutCommentLikes",
                table: "WorkoutCommentLikes",
                columns: new[] { "CommentId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutCommentLikes_CommentId",
                table: "WorkoutCommentLikes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutCommentLikes_CommentId_UserId",
                table: "WorkoutCommentLikes",
                columns: new[] { "CommentId", "UserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutCommentLikes_WorkoutComments_CommentId",
                table: "WorkoutCommentLikes",
                column: "CommentId",
                principalTable: "WorkoutComments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutCommentLikes_WorkoutComments_WorkoutCommentId",
                table: "WorkoutCommentLikes",
                column: "WorkoutCommentId",
                principalTable: "WorkoutComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
