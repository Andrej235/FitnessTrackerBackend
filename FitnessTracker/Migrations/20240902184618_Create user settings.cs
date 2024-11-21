using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class Createusersettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublicFollowing = table.Column<bool>(type: "bit", nullable: false),
                    PublicCompletedWorkouts = table.Column<bool>(type: "bit", nullable: false),
                    PublicStreak = table.Column<bool>(type: "bit", nullable: false),
                    PublicCurrentSplit = table.Column<bool>(type: "bit", nullable: false),
                    PublicLikedWorkouts = table.Column<bool>(type: "bit", nullable: false),
                    PublicFavoriteWorkouts = table.Column<bool>(type: "bit", nullable: false),
                    PublicLikedSplits = table.Column<bool>(type: "bit", nullable: false),
                    PublicFavoriteSplits = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_UserSettings", x => x.UserId);
                    _ = table.ForeignKey(
                        name: "FK_UserSettings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(
                name: "UserSettings");
    }
}
