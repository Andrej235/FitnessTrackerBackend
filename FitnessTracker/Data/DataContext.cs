using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<CompletedWorkout> CompletedWorkouts { get; set; } //
        public DbSet<EmailConfirmation> EmailConfirmations { get; set; } //
        public DbSet<Equipment> Equipment { get; set; } //
        public DbSet<EquipmentUsage> EquipmentUsage { get; set; } //
        public DbSet<Exercise> Exercises { get; set; } //
        public DbSet<FavoriteExercise> FavoriteExercises { get; set; } //
        public DbSet<FavoriteSplit> FavoriteSplits { get; set; } //
        public DbSet<FavoriteWorkout> FavoriteWorkouts { get; set; } //
        public DbSet<Muscle> Muscles { get; set; } //
        public DbSet<MuscleGroup> MusclesGroups { get; set; } //
        public DbSet<Post> Posts { get; set; } //
        public DbSet<PostComment> PostComments { get; set; } //
        public DbSet<PostComment> PostCommentLikes { get; set; } //
        public DbSet<PostLike> PostLikes { get; set; } //
        public DbSet<PrimaryMuscleGroupInExercise> PrimaryMuscleGroups { get; set; } //
        public DbSet<PrimaryMuscleInExercise> PrimaryMuscles { get; set; } //
        public DbSet<RefreshToken> RefreshTokens { get; set; } //
        public DbSet<SecondaryMuscleGroupInExercise> SecondaryMuscleGroups { get; set; } //
        public DbSet<SecondaryMuscleInExercise> SecondaryMuscles { get; set; } //
        public DbSet<Set> Sets { get; set; } //
        public DbSet<Split> Splits { get; set; } //
        public DbSet<SplitComment> SplitComments { get; set; } //
        public DbSet<SplitCommentLike> SplitCommentLikes { get; set; } //
        public DbSet<SplitLike> SplitLikes { get; set; } //
        public DbSet<SplitWorkout> SplitWorkouts { get; set; } //
        public DbSet<User> Users { get; set; } //
        public DbSet<UserFollows> UserFollows { get; set; } //
        public DbSet<Workout> Workouts { get; set; } //
        public DbSet<WorkoutComment> WorkoutComments { get; set; } //
        public DbSet<WorkoutCommentLike> WorkoutCommentLikes { get; set; } //
        public DbSet<WorkoutLike> WorkoutLikes { get; set; } //



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FitnessTracker;Integrated Security=True"); // TODO-PROD: Remove
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Ids
            modelBuilder.Entity<CompletedWorkout>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<EmailConfirmation>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Equipment>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Exercise>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Muscle>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<MuscleGroup>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Post>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<PostComment>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<RefreshToken>()
                .HasKey(p => p.Token);

            modelBuilder.Entity<Set>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Split>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<SplitComment>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Workout>()
                .HasKey(w => w.Id);

            modelBuilder.Entity<WorkoutComment>()
                .HasKey(w => w.Id);
            #endregion

            #region Completed workouts
            modelBuilder.Entity<CompletedWorkout>()
                .HasOne(cw => cw.Workout)
                .WithMany()
                .HasForeignKey(cw => cw.WorkoutId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CompletedWorkout>()
                .HasOne<User>()
                .WithMany(cw => cw.CompletedWorkouts)
                .HasForeignKey(cw => cw.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CompletedWorkout>()
                .HasIndex(x => x.UserId);

            modelBuilder.Entity<CompletedWorkout>()
                .HasIndex(x => new { x.UserId, x.WorkoutId });
            #endregion

            #region Email Confirmations
            modelBuilder.Entity<EmailConfirmation>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmailConfirmation>()
                .HasIndex(x => x.UserId);

            modelBuilder.Entity<EmailConfirmation>()
                .HasIndex(x => x.Id);
            #endregion

            #region Exercises
            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.PrimaryMuscles)
                .WithMany(m => m.PrimaryInExercises)
                .UsingEntity<PrimaryMuscleInExercise>(j =>
                {
                    j.HasOne<Exercise>().WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
                    j.HasOne<Muscle>().WithMany().HasForeignKey(x => x.MuscleId).OnDelete(DeleteBehavior.Cascade);
                    j.HasKey(x => new { x.ExerciseId, x.MuscleId });
                    j.HasIndex(x => x.ExerciseId);
                });

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.PrimaryMuscleGroups)
                .WithMany(m => m.PrimaryInExercises)
                .UsingEntity<PrimaryMuscleGroupInExercise>(j =>
                {
                    j.HasOne<Exercise>().WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
                    j.HasOne<MuscleGroup>().WithMany().HasForeignKey(x => x.MuscleGroupId).OnDelete(DeleteBehavior.Cascade);
                    j.HasKey(x => new { x.ExerciseId, x.MuscleGroupId });
                    j.HasIndex(x => x.ExerciseId);
                });

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.SecondaryMuscles)
                .WithMany(m => m.SecondaryInExercises)
                .UsingEntity<SecondaryMuscleInExercise>(j =>
                {
                    j.HasOne<Exercise>().WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
                    j.HasOne<Muscle>().WithMany().HasForeignKey(x => x.MuscleId).OnDelete(DeleteBehavior.Cascade);
                    j.HasKey(x => new { x.ExerciseId, x.MuscleId });
                    j.HasIndex(x => x.ExerciseId);
                });

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.SecondaryMuscleGroups)
                .WithMany(m => m.SecondaryInExercises)
                .UsingEntity<SecondaryMuscleGroupInExercise>(j =>
                {
                    j.HasOne<Exercise>().WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
                    j.HasOne<MuscleGroup>().WithMany().HasForeignKey(x => x.MuscleGroupId).OnDelete(DeleteBehavior.Cascade);
                    j.HasKey(x => new { x.ExerciseId, x.MuscleGroupId });
                    j.HasIndex(x => x.ExerciseId);
                });

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.Equipment)
                .WithMany(eq => eq.UsedInExercises)
                .UsingEntity<EquipmentUsage>(j =>
                {
                    j.HasOne<Exercise>().WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
                    j.HasOne<Equipment>().WithMany().HasForeignKey(x => x.EquipmentId).OnDelete(DeleteBehavior.Cascade);
                    j.HasKey(x => new { x.ExerciseId, x.EquipmentId });
                    j.HasIndex(x => x.ExerciseId);
                });

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.Favorites)
                .WithMany()
                .UsingEntity<FavoriteExercise>(j =>
                {
                    j.HasOne<Exercise>().WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
                    j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                    j.HasKey(x => new { x.ExerciseId, x.UserId });
                    j.HasIndex(x => x.ExerciseId);
                    j.HasIndex(x => new { x.ExerciseId, x.UserId }).IsUnique();
                });
            #endregion

            #region Muscles
            modelBuilder.Entity<Muscle>()
                .HasOne(m => m.MuscleGroup)
                .WithMany()
                .HasForeignKey(m => m.MuscleGroupId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Posts
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Likes)
                .WithMany()
                .UsingEntity<PostLike>(j =>
                {
                    j.HasOne<Post>().WithMany().HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.Cascade);
                    j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                    j.HasKey(x => new { x.PostId, x.UserId });
                    j.HasIndex(x => x.PostId);
                    j.HasIndex(x => new { x.PostId, x.UserId }).IsUnique();
                });

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne()
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Post Comments
            modelBuilder.Entity<PostComment>()
                .HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostComment>()
                .HasIndex(p => p.CreatorId);

            modelBuilder.Entity<PostComment>()
                .HasMany(p => p.Likes)
                .WithMany()
                .UsingEntity<PostCommentLike>(j =>
                {
                    j.HasOne<PostComment>().WithMany().HasForeignKey(x => x.PostCommentId).OnDelete(DeleteBehavior.NoAction);
                    j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
                    j.HasKey(x => new { x.PostCommentId, x.UserId });
                    j.HasIndex(x => x.PostCommentId);
                    j.HasIndex(x => new { x.PostCommentId, x.UserId }).IsUnique();
                });
            #endregion

            #region Refresh Tokens
            modelBuilder.Entity<RefreshToken>()
                .HasIndex(x => x.UserId);

            modelBuilder.Entity<RefreshToken>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Sets
            modelBuilder.Entity<Set>()
                .HasOne(s => s.Exercise)
                .WithMany()
                .HasForeignKey(s => s.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Set>()
                .HasOne<Workout>()
                .WithMany(w => w.Sets)
                .HasForeignKey(s => s.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Set>()
                .HasIndex(x => x.WorkoutId);
            #endregion

            #region Splits
            modelBuilder.Entity<Split>()
                .HasOne(s => s.Creator)
                .WithMany(u => u.CreatedSplits)
                .HasForeignKey(s => s.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Split>()
                .HasMany(s => s.Likes)
                .WithMany()
                .UsingEntity<SplitLike>(j =>
                {
                    j.HasOne<Split>().WithMany().HasForeignKey(x => x.SplitId).OnDelete(DeleteBehavior.Cascade);
                    j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                    j.HasKey(x => new { x.SplitId, x.UserId });
                    j.HasIndex(x => x.SplitId);
                    j.HasIndex(x => new { x.SplitId, x.UserId }).IsUnique();
                });

            modelBuilder.Entity<Split>()
                .HasMany(s => s.Comments)
                .WithOne()
                .HasForeignKey(s => s.SplitId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Split>()
                .HasMany(s => s.Workouts)
                .WithMany(w => w.Splits)
                .UsingEntity<SplitWorkout>(j =>
                {
                    j.HasOne<Split>().WithMany().HasForeignKey(x => x.SplitId).OnDelete(DeleteBehavior.Cascade);
                    j.HasOne<Workout>().WithMany().HasForeignKey(x => x.WorkoutId).OnDelete(DeleteBehavior.Cascade);
                    j.HasKey(x => new { x.SplitId, x.WorkoutId });
                    j.HasIndex(x => x.SplitId);
                });

            modelBuilder.Entity<Split>()
                .HasMany(s => s.Favorites)
                .WithMany()
                .UsingEntity<FavoriteSplit>(j =>
                {
                    j.HasOne<Split>().WithMany().HasForeignKey(x => x.SplitId).OnDelete(DeleteBehavior.Cascade);
                    j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                    j.HasKey(x => new { x.SplitId, x.UserId });
                    j.HasIndex(x => x.SplitId);
                    j.HasIndex(x => new { x.SplitId, x.UserId }).IsUnique();
                });
            #endregion

            #region Split comments
            modelBuilder.Entity<SplitComment>()
                .HasOne(s => s.Creator)
                .WithMany()
                .HasForeignKey(s => s.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SplitComment>()
                .HasIndex(x => x.SplitId);

            modelBuilder.Entity<SplitComment>()
                .HasMany(s => s.Likes)
                .WithMany()
                .UsingEntity<SplitCommentLike>(j =>
                {
                    j.HasOne<SplitComment>().WithMany().HasForeignKey(x => x.SplitCommentId).OnDelete(DeleteBehavior.NoAction);
                    j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
                    j.HasKey(x => new { x.SplitCommentId, x.UserId });
                    j.HasIndex(x => x.SplitCommentId);
                    j.HasIndex(x => new { x.SplitCommentId, x.UserId }).IsUnique();
                });
            #endregion

            #region Users
            modelBuilder.Entity<User>()
                .HasOne(u => u.CurrentSplit)
                .WithMany()
                .HasForeignKey(u => u.SplitId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Followers)
                .WithMany(u => u.Following)
                .UsingEntity<UserFollows>(j =>
                {

                    j.HasOne<User>().WithMany().HasForeignKey(x => x.FollowerId).OnDelete(DeleteBehavior.NoAction);
                    j.HasOne<User>().WithMany().HasForeignKey(x => x.FolloweeId).OnDelete(DeleteBehavior.NoAction);
                    j.HasKey(x => new { x.FollowerId, x.FolloweeId });
                    j.HasIndex(x => x.FollowerId);
                    j.HasIndex(x => x.FolloweeId);
                    j.HasIndex(x => new { x.FollowerId, x.FolloweeId }).IsUnique();
                });
            #endregion

            #region Workouts
            modelBuilder.Entity<Workout>()
                .HasOne(w => w.Creator)
                .WithMany(u => u.CreatedWorkouts)
                .HasForeignKey(u => u.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Workout>()
                .HasMany(w => w.Sets)
                .WithOne()
                .HasForeignKey(s => s.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Workout>()
                .HasMany(w => w.Likes)
                .WithMany()
                .UsingEntity<WorkoutLike>(j =>
                {
                    j.HasOne<Workout>().WithMany().HasForeignKey(x => x.WorkoutId).OnDelete(DeleteBehavior.NoAction);
                    j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
                    j.HasKey(x => new { x.WorkoutId, x.UserId });
                    j.HasIndex(x => x.WorkoutId);
                    j.HasIndex(x => new { x.WorkoutId, x.UserId }).IsUnique();
                });

            modelBuilder.Entity<Workout>()
                .HasMany(w => w.Comments)
                .WithOne()
                .HasForeignKey(c => c.WorkoutId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Workout>()
                .HasMany(w => w.Favorites)
                .WithMany()
                .UsingEntity<FavoriteWorkout>(j =>
                {
                    j.HasOne<Workout>().WithMany().HasForeignKey(x => x.WorkoutId).OnDelete(DeleteBehavior.NoAction);
                    j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
                    j.HasKey(x => new { x.WorkoutId, x.UserId });
                    j.HasIndex(x => x.WorkoutId);
                    j.HasIndex(x => new { x.WorkoutId, x.UserId }).IsUnique();
                });

            modelBuilder.Entity<Workout>()
                .HasIndex(x => x.CreatorId);
            #endregion

            #region Workout comments
            modelBuilder.Entity<WorkoutComment>()
                .HasOne(w => w.Creator)
                .WithMany()
                .HasForeignKey(w => w.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkoutComment>()
                .HasIndex(x => x.WorkoutId);

            modelBuilder.Entity<WorkoutComment>()
                .HasMany(w => w.Likes)
                .WithMany()
                .UsingEntity<WorkoutCommentLike>(j =>
                {
                    j.HasOne<WorkoutComment>().WithMany().HasForeignKey(x => x.WorkoutCommentId).OnDelete(DeleteBehavior.NoAction);
                    j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
                    j.HasKey(x => new { x.WorkoutCommentId, x.UserId });
                    j.HasIndex(x => x.WorkoutCommentId);
                    j.HasIndex(x => new { x.WorkoutCommentId, x.UserId }).IsUnique();
                });
            #endregion
        }
    }
}
