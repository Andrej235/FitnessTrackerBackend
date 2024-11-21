using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class RemovePostsandrelatedtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "PostCommentLike");

            _ = migrationBuilder.DropTable(
                name: "PostLikes");

            _ = migrationBuilder.DropTable(
                name: "PostComment");

            _ = migrationBuilder.DropTable(
                name: "Posts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Posts", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Posts_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateTable(
                name: "PostComment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_PostComment", x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_PostComment_PostComment_ParentId",
                        column: x => x.ParentId,
                        principalTable: "PostComment",
                        principalColumn: "Id");
                    _ = table.ForeignKey(
                        name: "FK_PostComment_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_PostComment_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "PostLikes",
                columns: table => new
                {
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_PostLikes", x => new { x.PostId, x.UserId });
                    _ = table.ForeignKey(
                        name: "FK_PostLikes_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_PostLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "PostCommentLike",
                columns: table => new
                {
                    PostCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_PostCommentLike", x => new { x.PostCommentId, x.UserId });
                    _ = table.ForeignKey(
                        name: "FK_PostCommentLike_PostComment_PostCommentId",
                        column: x => x.PostCommentId,
                        principalTable: "PostComment",
                        principalColumn: "Id");
                    _ = table.ForeignKey(
                        name: "FK_PostCommentLike_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_PostComment_CreatorId",
                table: "PostComment",
                column: "CreatorId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_PostComment_ParentId",
                table: "PostComment",
                column: "ParentId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_PostComment_PostId",
                table: "PostComment",
                column: "PostId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_PostCommentLike_PostCommentId",
                table: "PostCommentLike",
                column: "PostCommentId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_PostCommentLike_PostCommentId_UserId",
                table: "PostCommentLike",
                columns: new[] { "PostCommentId", "UserId" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_PostCommentLike_UserId",
                table: "PostCommentLike",
                column: "UserId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_PostLikes_PostId",
                table: "PostLikes",
                column: "PostId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_PostLikes_PostId_UserId",
                table: "PostLikes",
                columns: new[] { "PostId", "UserId" },
                unique: true);

            _ = migrationBuilder.CreateIndex(
                name: "IX_PostLikes_UserId",
                table: "PostLikes",
                column: "UserId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Posts_CreatorId",
                table: "Posts",
                column: "CreatorId");
        }
    }
}
