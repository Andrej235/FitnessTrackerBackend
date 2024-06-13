using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class Removeweightsaliasesandworkoutsets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Muscles_MuscleGroups_MuscleGroupId",
                table: "Muscles");

            migrationBuilder.DropForeignKey(
                name: "FK_Sets_Users_CreatorId",
                table: "Sets");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Exercises_ExerciseId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Aliases");

            migrationBuilder.DropTable(
                name: "Weights");

            migrationBuilder.DropTable(
                name: "WorkoutSets");

            migrationBuilder.DropIndex(
                name: "IX_Users_ExerciseId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Muscles_MuscleGroupId",
                table: "Muscles");

            migrationBuilder.DropColumn(
                name: "ExerciseId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DropSet",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "ToFaliure",
                table: "Sets");

            migrationBuilder.RenameColumn(
                name: "RepRange_Top",
                table: "Sets",
                newName: "TopRepRange");

            migrationBuilder.RenameColumn(
                name: "RepRange_Bottom",
                table: "Sets",
                newName: "NumberOfSets");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Sets",
                newName: "WorkoutId");

            migrationBuilder.RenameIndex(
                name: "IX_Sets_CreatorId",
                table: "Sets",
                newName: "IX_Sets_WorkoutId");

            migrationBuilder.AddColumn<int>(
                name: "BottomRepRange",
                table: "Sets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Sets_Workouts_WorkoutId",
                table: "Sets",
                column: "WorkoutId",
                principalTable: "Workouts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sets_Workouts_WorkoutId",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "BottomRepRange",
                table: "Sets");

            migrationBuilder.RenameColumn(
                name: "WorkoutId",
                table: "Sets",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "TopRepRange",
                table: "Sets",
                newName: "RepRange_Top");

            migrationBuilder.RenameColumn(
                name: "NumberOfSets",
                table: "Sets",
                newName: "RepRange_Bottom");

            migrationBuilder.RenameIndex(
                name: "IX_Sets_WorkoutId",
                table: "Sets",
                newName: "IX_Sets_CreatorId");

            migrationBuilder.AddColumn<int>(
                name: "ExerciseId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DropSet",
                table: "Sets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ToFaliure",
                table: "Sets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Aliases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    AliasName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aliases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aliases_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateOfAchieving = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weights_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Weights_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetSets = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutSets_Sets_SetId",
                        column: x => x.SetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutSets_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ExerciseId",
                table: "Users",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_Muscles_MuscleGroupId",
                table: "Muscles",
                column: "MuscleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Aliases_ExerciseId",
                table: "Aliases",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_Weights_ExerciseId",
                table: "Weights",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_Weights_UserId",
                table: "Weights",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSets_SetId",
                table: "WorkoutSets",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSets_WorkoutId",
                table: "WorkoutSets",
                column: "WorkoutId");

            migrationBuilder.AddForeignKey(
                name: "FK_Muscles_MuscleGroups_MuscleGroupId",
                table: "Muscles",
                column: "MuscleGroupId",
                principalTable: "MuscleGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sets_Users_CreatorId",
                table: "Sets",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Exercises_ExerciseId",
                table: "Users",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id");
        }
    }
}
