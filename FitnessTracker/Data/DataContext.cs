using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentChild> CommentChildren { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<CompletedWorkout> CompletedWorkouts { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<EquipmentUsage> EquipmentUsage { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Muscle> Muscles { get; set; }
        public DbSet<MuscleGroup> MusclesGroups { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<PrimaryMuscleGroupInExercise> PrimaryMuscleGroups { get; set; }
        public DbSet<PrimaryMuscleInExercise> PrimaryMuscles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<SecondaryMuscleGroupInExercise> SecondaryMuscleGroups { get; set; }
        public DbSet<SecondaryMuscleInExercise> SecondaryMuscles { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<Split> Splits { get; set; }
        public DbSet<SplitComment> SplitComments { get; set; }
        public DbSet<SplitLike> SplitLikes { get; set; }
        public DbSet<SplitWorkout> SplitWorkouts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFollows> UserFollows { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutComment> WorkoutComments { get; set; }
        public DbSet<WorkoutLike> WorkoutLikes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
