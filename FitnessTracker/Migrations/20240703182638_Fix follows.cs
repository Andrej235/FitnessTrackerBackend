using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class Fixfollows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.AddColumn<DateTime>(
                name: "JoinedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            _ = migrationBuilder.AddColumn<Guid>(
                name: "FollowersId",
                table: "UserFollows",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            _ = migrationBuilder.CreateIndex(
                name: "IX_UserFollows_FollowersId",
                table: "UserFollows",
                column: "FollowersId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_UserFollows_Users_FollowersId",
                table: "UserFollows",
                column: "FollowersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_UserFollows_Users_FollowersId",
                table: "UserFollows");

            _ = migrationBuilder.DropIndex(
                name: "IX_UserFollows_FollowersId",
                table: "UserFollows");

            _ = migrationBuilder.DropColumn(
                name: "JoinedAt",
                table: "Users");

            _ = migrationBuilder.DropColumn(
                name: "FollowersId",
                table: "UserFollows");
        }
    }
}
