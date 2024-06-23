using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class Fixallcommentliketables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCommentLike_PostComment_CommentId",
                table: "PostCommentLike");

            migrationBuilder.DropForeignKey(
                name: "FK_PostCommentLike_PostComment_PostCommentId",
                table: "PostCommentLike");

            migrationBuilder.DropForeignKey(
                name: "FK_SplitCommentLikes_SplitComments_CommentId",
                table: "SplitCommentLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_SplitCommentLikes_SplitComments_SplitCommentId",
                table: "SplitCommentLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SplitCommentLikes",
                table: "SplitCommentLikes");

            migrationBuilder.DropIndex(
                name: "IX_SplitCommentLikes_CommentId",
                table: "SplitCommentLikes");

            migrationBuilder.DropIndex(
                name: "IX_SplitCommentLikes_CommentId_UserId",
                table: "SplitCommentLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostCommentLike",
                table: "PostCommentLike");

            migrationBuilder.DropIndex(
                name: "IX_PostCommentLike_CommentId",
                table: "PostCommentLike");

            migrationBuilder.DropIndex(
                name: "IX_PostCommentLike_CommentId_UserId",
                table: "PostCommentLike");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "SplitCommentLikes");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "PostCommentLike");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SplitCommentLikes",
                table: "SplitCommentLikes",
                columns: new[] { "SplitCommentId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostCommentLike",
                table: "PostCommentLike",
                columns: new[] { "PostCommentId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_SplitCommentLikes_SplitCommentId_UserId",
                table: "SplitCommentLikes",
                columns: new[] { "SplitCommentId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostCommentLike_PostCommentId_UserId",
                table: "PostCommentLike",
                columns: new[] { "PostCommentId", "UserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostCommentLike_PostComment_PostCommentId",
                table: "PostCommentLike",
                column: "PostCommentId",
                principalTable: "PostComment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SplitCommentLikes_SplitComments_SplitCommentId",
                table: "SplitCommentLikes",
                column: "SplitCommentId",
                principalTable: "SplitComments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCommentLike_PostComment_PostCommentId",
                table: "PostCommentLike");

            migrationBuilder.DropForeignKey(
                name: "FK_SplitCommentLikes_SplitComments_SplitCommentId",
                table: "SplitCommentLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SplitCommentLikes",
                table: "SplitCommentLikes");

            migrationBuilder.DropIndex(
                name: "IX_SplitCommentLikes_SplitCommentId_UserId",
                table: "SplitCommentLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostCommentLike",
                table: "PostCommentLike");

            migrationBuilder.DropIndex(
                name: "IX_PostCommentLike_PostCommentId_UserId",
                table: "PostCommentLike");

            migrationBuilder.AddColumn<Guid>(
                name: "CommentId",
                table: "SplitCommentLikes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CommentId",
                table: "PostCommentLike",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_SplitCommentLikes",
                table: "SplitCommentLikes",
                columns: new[] { "CommentId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostCommentLike",
                table: "PostCommentLike",
                columns: new[] { "CommentId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_SplitCommentLikes_CommentId",
                table: "SplitCommentLikes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_SplitCommentLikes_CommentId_UserId",
                table: "SplitCommentLikes",
                columns: new[] { "CommentId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostCommentLike_CommentId",
                table: "PostCommentLike",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_PostCommentLike_CommentId_UserId",
                table: "PostCommentLike",
                columns: new[] { "CommentId", "UserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostCommentLike_PostComment_CommentId",
                table: "PostCommentLike",
                column: "CommentId",
                principalTable: "PostComment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostCommentLike_PostComment_PostCommentId",
                table: "PostCommentLike",
                column: "PostCommentId",
                principalTable: "PostComment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SplitCommentLikes_SplitComments_CommentId",
                table: "SplitCommentLikes",
                column: "CommentId",
                principalTable: "SplitComments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SplitCommentLikes_SplitComments_SplitCommentId",
                table: "SplitCommentLikes",
                column: "SplitCommentId",
                principalTable: "SplitComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
