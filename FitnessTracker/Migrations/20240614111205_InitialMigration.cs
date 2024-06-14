using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MusclesGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusclesGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUsage",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUsage", x => new { x.ExerciseId, x.EquipmentId });
                    table.ForeignKey(
                        name: "FK_EquipmentUsage_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentUsage_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Muscles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MuscleGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Muscles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Muscles_MusclesGroups_MuscleGroupId",
                        column: x => x.MuscleGroupId,
                        principalTable: "MusclesGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryMuscleGroups",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    MuscleGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryMuscleGroups", x => new { x.ExerciseId, x.MuscleGroupId });
                    table.ForeignKey(
                        name: "FK_PrimaryMuscleGroups_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrimaryMuscleGroups_MusclesGroups_MuscleGroupId",
                        column: x => x.MuscleGroupId,
                        principalTable: "MusclesGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecondaryMuscleGroups",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    MuscleGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecondaryMuscleGroups", x => new { x.ExerciseId, x.MuscleGroupId });
                    table.ForeignKey(
                        name: "FK_SecondaryMuscleGroups_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecondaryMuscleGroups_MusclesGroups_MuscleGroupId",
                        column: x => x.MuscleGroupId,
                        principalTable: "MusclesGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryMuscles",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    MuscleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryMuscles", x => new { x.ExerciseId, x.MuscleId });
                    table.ForeignKey(
                        name: "FK_PrimaryMuscles_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrimaryMuscles_Muscles_MuscleId",
                        column: x => x.MuscleId,
                        principalTable: "Muscles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecondaryMuscles",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    MuscleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecondaryMuscles", x => new { x.ExerciseId, x.MuscleId });
                    table.ForeignKey(
                        name: "FK_SecondaryMuscles_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecondaryMuscles_Muscles_MuscleId",
                        column: x => x.MuscleId,
                        principalTable: "Muscles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompletedWorkouts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedWorkouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteExercises",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteExercises", x => new { x.ExerciseId, x.UserId });
                    table.ForeignKey(
                        name: "FK_FavoriteExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteSplits",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SplitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteSplits", x => new { x.SplitId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "FavoriteWorkouts",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteWorkouts", x => new { x.WorkoutId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "PostComment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PostCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostComment_PostComment_PostCommentId",
                        column: x => x.PostCommentId,
                        principalTable: "PostComment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PostCommentLike",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCommentLike", x => new { x.CommentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_PostCommentLike_PostComment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "PostComment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PostCommentLike_PostComment_PostCommentId",
                        column: x => x.PostCommentId,
                        principalTable: "PostComment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostLikes",
                columns: table => new
                {
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLikes", x => new { x.PostId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Token = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JwtId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Token);
                });

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopRepRange = table.Column<int>(type: "int", nullable: false),
                    BottomRepRange = table.Column<int>(type: "int", nullable: false),
                    NumberOfSets = table.Column<int>(type: "int", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    WorkoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sets_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SplitCommentLikes",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SplitCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SplitCommentLikes", x => new { x.CommentId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "SplitComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SplitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SplitCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SplitComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SplitComments_SplitComments_SplitCommentId",
                        column: x => x.SplitCommentId,
                        principalTable: "SplitComments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SplitLikes",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SplitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SplitLikes", x => new { x.SplitId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "Splits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Splits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Salt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ProfilePic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SplitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Splits_SplitId",
                        column: x => x.SplitId,
                        principalTable: "Splits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserFollows",
                columns: table => new
                {
                    FollowerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FolloweeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollows", x => new { x.FollowerId, x.FolloweeId });
                    table.ForeignKey(
                        name: "FK_UserFollows_Users_FolloweeId",
                        column: x => x.FolloweeId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserFollows_Users_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserFollows_Users_FollowingId",
                        column: x => x.FollowingId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Workouts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workouts_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SplitWorkouts",
                columns: table => new
                {
                    WorkoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SplitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SplitWorkouts", x => new { x.SplitId, x.WorkoutId });
                    table.ForeignKey(
                        name: "FK_SplitWorkouts_Splits_SplitId",
                        column: x => x.SplitId,
                        principalTable: "Splits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SplitWorkouts_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkoutCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutComments_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutComments_WorkoutComments_WorkoutCommentId",
                        column: x => x.WorkoutCommentId,
                        principalTable: "WorkoutComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkoutComments_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkoutLikes",
                columns: table => new
                {
                    WorkoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutLikes", x => new { x.WorkoutId, x.UserId });
                    table.ForeignKey(
                        name: "FK_WorkoutLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkoutLikes_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkoutCommentLikes",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkoutCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutCommentLikes", x => new { x.CommentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_WorkoutCommentLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkoutCommentLikes_WorkoutComments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "WorkoutComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkoutCommentLikes_WorkoutComments_WorkoutCommentId",
                        column: x => x.WorkoutCommentId,
                        principalTable: "WorkoutComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedWorkouts_UserId",
                table: "CompletedWorkouts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedWorkouts_WorkoutId",
                table: "CompletedWorkouts",
                column: "WorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUsage_EquipmentId",
                table: "EquipmentUsage",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteExercises_ExerciseId",
                table: "FavoriteExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteExercises_ExerciseId_UserId",
                table: "FavoriteExercises",
                columns: new[] { "ExerciseId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteExercises_UserId",
                table: "FavoriteExercises",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteSplits_SplitId",
                table: "FavoriteSplits",
                column: "SplitId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteSplits_SplitId_UserId",
                table: "FavoriteSplits",
                columns: new[] { "SplitId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteSplits_UserId",
                table: "FavoriteSplits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteWorkouts_UserId",
                table: "FavoriteWorkouts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteWorkouts_WorkoutId",
                table: "FavoriteWorkouts",
                column: "WorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteWorkouts_WorkoutId_UserId",
                table: "FavoriteWorkouts",
                columns: new[] { "WorkoutId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Muscles_MuscleGroupId",
                table: "Muscles",
                column: "MuscleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComment_CreatorId",
                table: "PostComment",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComment_PostCommentId",
                table: "PostComment",
                column: "PostCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComment_PostId",
                table: "PostComment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostCommentLike_CommentId",
                table: "PostCommentLike",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_PostCommentLike_CommentId_UserId",
                table: "PostCommentLike",
                columns: new[] { "CommentId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostCommentLike_PostCommentId",
                table: "PostCommentLike",
                column: "PostCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_PostCommentLike_UserId",
                table: "PostCommentLike",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_PostId",
                table: "PostLikes",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_PostId_UserId",
                table: "PostLikes",
                columns: new[] { "PostId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_UserId",
                table: "PostLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CreatorId",
                table: "Posts",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryMuscleGroups_MuscleGroupId",
                table: "PrimaryMuscleGroups",
                column: "MuscleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryMuscles_MuscleId",
                table: "PrimaryMuscles",
                column: "MuscleId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SecondaryMuscleGroups_MuscleGroupId",
                table: "SecondaryMuscleGroups",
                column: "MuscleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SecondaryMuscles_MuscleId",
                table: "SecondaryMuscles",
                column: "MuscleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_ExerciseId",
                table: "Sets",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_WorkoutId",
                table: "Sets",
                column: "WorkoutId");

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
                name: "IX_SplitCommentLikes_SplitCommentId",
                table: "SplitCommentLikes",
                column: "SplitCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_SplitCommentLikes_UserId",
                table: "SplitCommentLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SplitComments_CreatorId",
                table: "SplitComments",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SplitComments_SplitCommentId",
                table: "SplitComments",
                column: "SplitCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_SplitComments_SplitId",
                table: "SplitComments",
                column: "SplitId");

            migrationBuilder.CreateIndex(
                name: "IX_SplitLikes_SplitId",
                table: "SplitLikes",
                column: "SplitId");

            migrationBuilder.CreateIndex(
                name: "IX_SplitLikes_SplitId_UserId",
                table: "SplitLikes",
                columns: new[] { "SplitId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SplitLikes_UserId",
                table: "SplitLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Splits_CreatorId",
                table: "Splits",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SplitWorkouts_SplitId",
                table: "SplitWorkouts",
                column: "SplitId");

            migrationBuilder.CreateIndex(
                name: "IX_SplitWorkouts_WorkoutId",
                table: "SplitWorkouts",
                column: "WorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollows_FolloweeId",
                table: "UserFollows",
                column: "FolloweeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollows_FollowerId",
                table: "UserFollows",
                column: "FollowerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollows_FollowerId_FolloweeId",
                table: "UserFollows",
                columns: new[] { "FollowerId", "FolloweeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFollows_FollowingId",
                table: "UserFollows",
                column: "FollowingId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SplitId",
                table: "Users",
                column: "SplitId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutCommentLikes_CommentId",
                table: "WorkoutCommentLikes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutCommentLikes_CommentId_UserId",
                table: "WorkoutCommentLikes",
                columns: new[] { "CommentId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutCommentLikes_UserId",
                table: "WorkoutCommentLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutCommentLikes_WorkoutCommentId",
                table: "WorkoutCommentLikes",
                column: "WorkoutCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutComments_CreatorId",
                table: "WorkoutComments",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutComments_WorkoutCommentId",
                table: "WorkoutComments",
                column: "WorkoutCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutComments_WorkoutId",
                table: "WorkoutComments",
                column: "WorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLikes_UserId",
                table: "WorkoutLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLikes_WorkoutId",
                table: "WorkoutLikes",
                column: "WorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLikes_WorkoutId_UserId",
                table: "WorkoutLikes",
                columns: new[] { "WorkoutId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_CreatorId",
                table: "Workouts",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedWorkouts_Users_UserId",
                table: "CompletedWorkouts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedWorkouts_Workouts_WorkoutId",
                table: "CompletedWorkouts",
                column: "WorkoutId",
                principalTable: "Workouts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteExercises_Users_UserId",
                table: "FavoriteExercises",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteSplits_Splits_SplitId",
                table: "FavoriteSplits",
                column: "SplitId",
                principalTable: "Splits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteSplits_Users_UserId",
                table: "FavoriteSplits",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteWorkouts_Users_UserId",
                table: "FavoriteWorkouts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteWorkouts_Workouts_WorkoutId",
                table: "FavoriteWorkouts",
                column: "WorkoutId",
                principalTable: "Workouts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComment_Posts_PostId",
                table: "PostComment",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostComment_Users_CreatorId",
                table: "PostComment",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostCommentLike_Users_UserId",
                table: "PostCommentLike",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostLikes_Posts_PostId",
                table: "PostLikes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostLikes_Users_UserId",
                table: "PostLikes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_CreatorId",
                table: "Posts",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sets_Workouts_WorkoutId",
                table: "Sets",
                column: "WorkoutId",
                principalTable: "Workouts",
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

            migrationBuilder.AddForeignKey(
                name: "FK_SplitCommentLikes_Users_UserId",
                table: "SplitCommentLikes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SplitComments_Splits_SplitId",
                table: "SplitComments",
                column: "SplitId",
                principalTable: "Splits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SplitComments_Users_CreatorId",
                table: "SplitComments",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SplitLikes_Splits_SplitId",
                table: "SplitLikes",
                column: "SplitId",
                principalTable: "Splits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SplitLikes_Users_UserId",
                table: "SplitLikes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Splits_Users_CreatorId",
                table: "Splits",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Splits_Users_CreatorId",
                table: "Splits");

            migrationBuilder.DropTable(
                name: "CompletedWorkouts");

            migrationBuilder.DropTable(
                name: "EquipmentUsage");

            migrationBuilder.DropTable(
                name: "FavoriteExercises");

            migrationBuilder.DropTable(
                name: "FavoriteSplits");

            migrationBuilder.DropTable(
                name: "FavoriteWorkouts");

            migrationBuilder.DropTable(
                name: "PostCommentLike");

            migrationBuilder.DropTable(
                name: "PostLikes");

            migrationBuilder.DropTable(
                name: "PrimaryMuscleGroups");

            migrationBuilder.DropTable(
                name: "PrimaryMuscles");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "SecondaryMuscleGroups");

            migrationBuilder.DropTable(
                name: "SecondaryMuscles");

            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.DropTable(
                name: "SplitCommentLikes");

            migrationBuilder.DropTable(
                name: "SplitLikes");

            migrationBuilder.DropTable(
                name: "SplitWorkouts");

            migrationBuilder.DropTable(
                name: "UserFollows");

            migrationBuilder.DropTable(
                name: "WorkoutCommentLikes");

            migrationBuilder.DropTable(
                name: "WorkoutLikes");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "PostComment");

            migrationBuilder.DropTable(
                name: "Muscles");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "SplitComments");

            migrationBuilder.DropTable(
                name: "WorkoutComments");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "MusclesGroups");

            migrationBuilder.DropTable(
                name: "Workouts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Splits");
        }
    }
}
