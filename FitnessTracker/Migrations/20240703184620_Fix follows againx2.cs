using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class Fixfollowsagainx2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_Users_FollowersId",
                table: "UserFollows");

            _ = migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_Users_FollowingId",
                table: "UserFollows");

            _ = migrationBuilder.DropIndex(
                name: "IX_UserFollows_FollowersId",
                table: "UserFollows");

            _ = migrationBuilder.DropIndex(
                name: "IX_UserFollows_FollowingId",
                table: "UserFollows");

            _ = migrationBuilder.DropColumn(
                name: "FollowersId",
                table: "UserFollows");

            _ = migrationBuilder.DropColumn(
                name: "FollowingId",
                table: "UserFollows");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<Guid>(
                name: "FollowersId",
                table: "UserFollows",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            _ = migrationBuilder.AddColumn<Guid>(
                name: "FollowingId",
                table: "UserFollows",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            _ = migrationBuilder.CreateIndex(
                name: "IX_UserFollows_FollowersId",
                table: "UserFollows",
                column: "FollowersId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_UserFollows_FollowingId",
                table: "UserFollows",
                column: "FollowingId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_Users_FollowersId",
                table: "UserFollows",
                column: "FollowersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            _ = migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_Users_FollowingId",
                table: "UserFollows",
                column: "FollowingId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
