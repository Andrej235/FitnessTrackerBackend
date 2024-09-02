using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<CompletedWorkout> CompletedWorkouts { get; set; } //
        public DbSet<CompletedSet> CompletedSets { get; set; }
        public DbSet<EmailConfirmation> EmailConfirmations { get; set; } //
        public DbSet<Equipment> Equipment { get; set; } //
        public DbSet<EquipmentUsage> EquipmentUsage { get; set; } //
        public DbSet<Exercise> Exercises { get; set; } //
        public DbSet<FavoriteExercise> FavoriteExercises { get; set; } //
        public DbSet<FavoriteSplit> FavoriteSplits { get; set; } //
        public DbSet<FavoriteWorkout> FavoriteWorkouts { get; set; } //
        public DbSet<Muscle> Muscles { get; set; } //
        public DbSet<MuscleGroup> MusclesGroups { get; set; } //
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



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.Entity<CompletedWorkout>(completedWorkout =>
            {
                _ = completedWorkout.HasKey(c => c.Id);

                _ = completedWorkout.HasOne(cw => cw.Workout)
                    .WithMany()
                    .HasForeignKey(cw => cw.WorkoutId)
                    .OnDelete(DeleteBehavior.NoAction);

                _ = completedWorkout.HasOne<User>()
                    .WithMany(cw => cw.CompletedWorkouts)
                    .HasForeignKey(cw => cw.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                _ = completedWorkout.HasOne(cw => cw.Split)
                    .WithMany()
                    .HasForeignKey(cw => cw.SplitId)
                    .OnDelete(DeleteBehavior.NoAction);

                _ = completedWorkout.HasIndex(x => x.UserId);
                _ = completedWorkout.HasIndex(x => new { x.UserId, x.WorkoutId });
            });

            _ = modelBuilder.Entity<CompletedSet>(completedSet =>
            {
                _ = completedSet.HasKey(cs => cs.Id);

                _ = completedSet.HasOne(cs => cs.CompletedWorkout)
                    .WithMany(cw => cw.CompletedSets)
                    .HasForeignKey(cs => cs.CompletedWorkoutId)
                    .OnDelete(DeleteBehavior.NoAction);

                _ = completedSet.HasOne<User>()
                    .WithMany(cs => cs.CompletedSets)
                    .HasForeignKey(cs => cs.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                _ = completedSet.HasOne(cs => cs.Set)
                    .WithMany()
                    .HasForeignKey(cs => cs.SetId)
                    .OnDelete(DeleteBehavior.NoAction);

                _ = completedSet.HasIndex(x => x.UserId);
                _ = completedSet.HasIndex(x => x.CompletedWorkoutId);
                _ = completedSet.HasIndex(x => new { x.UserId, x.SetId });
            });

            _ = modelBuilder.Entity<EmailConfirmation>(emailConformation =>
            {
                _ = emailConformation.HasKey(e => e.Id);

                _ = emailConformation.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                _ = emailConformation.HasIndex(x => x.Id).IsUnique();
                _ = emailConformation.HasIndex(x => x.UserId);
            });

            _ = modelBuilder.Entity<Equipment>(equipment => equipment.HasKey(e => e.Id));

            _ = modelBuilder.Entity<Exercise>(exercise =>
            {
                _ = exercise.HasKey(e => e.Id);

                _ = exercise.HasMany(e => e.PrimaryMuscles)
                    .WithMany(m => m.PrimaryInExercises)
                    .UsingEntity<PrimaryMuscleInExercise>(j =>
                    {
                        _ = j.HasOne<Exercise>().WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
                        _ = j.HasOne<Muscle>().WithMany().HasForeignKey(x => x.MuscleId).OnDelete(DeleteBehavior.Cascade);
                        _ = j.HasKey(x => new { x.ExerciseId, x.MuscleId });
                        _ = j.HasIndex(x => x.ExerciseId);
                    });

                _ = exercise.HasMany(e => e.PrimaryMuscleGroups)
                    .WithMany(m => m.PrimaryInExercises)
                    .UsingEntity<PrimaryMuscleGroupInExercise>(j =>
                    {
                        _ = j.HasOne<Exercise>().WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
                        _ = j.HasOne<MuscleGroup>().WithMany().HasForeignKey(x => x.MuscleGroupId).OnDelete(DeleteBehavior.Cascade);
                        _ = j.HasKey(x => new { x.ExerciseId, x.MuscleGroupId });
                        _ = j.HasIndex(x => x.ExerciseId);
                    });

                _ = exercise.HasMany(e => e.SecondaryMuscles)
                    .WithMany(m => m.SecondaryInExercises)
                    .UsingEntity<SecondaryMuscleInExercise>(j =>
                    {
                        _ = j.HasOne<Exercise>().WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
                        _ = j.HasOne<Muscle>().WithMany().HasForeignKey(x => x.MuscleId).OnDelete(DeleteBehavior.Cascade);
                        _ = j.HasKey(x => new { x.ExerciseId, x.MuscleId });
                        _ = j.HasIndex(x => x.ExerciseId);
                    });

                _ = exercise.HasMany(e => e.SecondaryMuscleGroups)
                    .WithMany(m => m.SecondaryInExercises)
                    .UsingEntity<SecondaryMuscleGroupInExercise>(j =>
                    {
                        _ = j.HasOne<Exercise>().WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
                        _ = j.HasOne<MuscleGroup>().WithMany().HasForeignKey(x => x.MuscleGroupId).OnDelete(DeleteBehavior.Cascade);
                        _ = j.HasKey(x => new { x.ExerciseId, x.MuscleGroupId });
                        _ = j.HasIndex(x => x.ExerciseId);
                    });

                _ = exercise.HasMany(e => e.Equipment)
                    .WithMany(eq => eq.UsedInExercises)
                    .UsingEntity<EquipmentUsage>(j =>
                    {
                        _ = j.HasOne<Exercise>().WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
                        _ = j.HasOne<Equipment>().WithMany().HasForeignKey(x => x.EquipmentId).OnDelete(DeleteBehavior.Cascade);
                        _ = j.HasKey(x => new { x.ExerciseId, x.EquipmentId });
                        _ = j.HasIndex(x => x.ExerciseId);
                    });

                _ = exercise.HasMany(e => e.Favorites)
                    .WithMany()
                    .UsingEntity<FavoriteExercise>(j =>
                    {
                        _ = j.HasOne<Exercise>().WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
                        _ = j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                        _ = j.HasKey(x => new { x.ExerciseId, x.UserId });
                        _ = j.HasIndex(x => x.ExerciseId);
                        _ = j.HasIndex(x => new { x.ExerciseId, x.UserId }).IsUnique();
                    });
            });

            _ = modelBuilder.Entity<Muscle>(muscle =>
            {
                _ = muscle.HasKey(m => m.Id);

                _ = muscle.HasOne(m => m.MuscleGroup)
                    .WithMany(mg => mg.Muscles)
                    .HasForeignKey(m => m.MuscleGroupId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            _ = modelBuilder.Entity<MuscleGroup>(muscleGroup => muscleGroup.HasKey(mg => mg.Id));

            _ = modelBuilder.Entity<RefreshToken>(refreshToken =>
            {
                _ = refreshToken.HasKey(p => p.Token);

                _ = refreshToken.HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                _ = refreshToken.HasIndex(x => x.UserId);
            });

            _ = modelBuilder.Entity<Set>(set =>
            {
                _ = set.HasKey(x => x.Id);

                _ = set.HasOne(s => s.Exercise)
                    .WithMany()
                    .HasForeignKey(s => s.ExerciseId)
                    .OnDelete(DeleteBehavior.Cascade);

                _ = set.HasOne<Workout>()
                    .WithMany(w => w.Sets)
                    .HasForeignKey(s => s.WorkoutId)
                    .OnDelete(DeleteBehavior.Cascade);

                _ = set.HasIndex(x => x.WorkoutId);
            });

            _ = modelBuilder.Entity<Split>(split =>
            {
                _ = split.HasKey(s => s.Id);

                _ = split.HasOne(s => s.Creator)
                    .WithMany(u => u.CreatedSplits)
                    .HasForeignKey(s => s.CreatorId)
                    .OnDelete(DeleteBehavior.NoAction);

                _ = split.HasMany(s => s.Likes)
                    .WithMany()
                    .UsingEntity<SplitLike>(j =>
                    {
                        _ = j.HasOne<Split>().WithMany().HasForeignKey(x => x.SplitId).OnDelete(DeleteBehavior.Cascade);
                        _ = j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                        _ = j.HasKey(x => new { x.SplitId, x.UserId });
                        _ = j.HasIndex(x => x.SplitId);
                        _ = j.HasIndex(x => new { x.SplitId, x.UserId }).IsUnique();
                    });

                _ = split.HasMany(s => s.Comments)
                    .WithOne()
                    .HasForeignKey(s => s.SplitId)
                    .OnDelete(DeleteBehavior.Cascade);

                _ = split.HasMany(s => s.Workouts)
                    .WithOne(sw => sw.Split)
                    .HasForeignKey(sw => sw.SplitId)
                    .OnDelete(DeleteBehavior.Cascade);

                _ = split.HasMany(s => s.Favorites)
                    .WithMany()
                    .UsingEntity<FavoriteSplit>(j =>
                    {
                        _ = j.HasOne<Split>().WithMany().HasForeignKey(x => x.SplitId).OnDelete(DeleteBehavior.Cascade);
                        _ = j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                        _ = j.HasKey(x => new { x.SplitId, x.UserId });
                        _ = j.HasIndex(x => x.SplitId);
                        _ = j.HasIndex(x => new { x.SplitId, x.UserId }).IsUnique();
                    });
            });

            _ = modelBuilder.Entity<SplitComment>(splitComment =>
            {
                _ = splitComment.HasKey(sc => sc.Id);

                _ = splitComment.HasOne(sc => sc.Creator)
                    .WithMany()
                    .HasForeignKey(sc => sc.CreatorId)
                    .OnDelete(DeleteBehavior.Cascade);

                _ = splitComment.HasMany(sc => sc.Children)
                    .WithOne()
                    .IsRequired(false)
                    .HasForeignKey(sc => sc.ParentId)
                    .OnDelete(DeleteBehavior.NoAction);

                _ = splitComment.HasMany(sc => sc.Likes)
                    .WithMany()
                    .UsingEntity<SplitCommentLike>(j =>
                    {
                        _ = j.HasOne<SplitComment>().WithMany().HasForeignKey(x => x.SplitCommentId).OnDelete(DeleteBehavior.NoAction);
                        _ = j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
                        _ = j.HasKey(x => new { x.SplitCommentId, x.UserId });
                        _ = j.HasIndex(x => x.SplitCommentId);
                        _ = j.HasIndex(x => new { x.SplitCommentId, x.UserId }).IsUnique();
                    });

                _ = splitComment.HasIndex(sc => sc.SplitId);
            });

            _ = modelBuilder.Entity<SplitWorkout>(entity =>
            {
                _ = entity.HasKey(x => new { x.SplitId, x.Day });

                _ = entity.HasOne(sw => sw.Split)
                    .WithMany(s => s.Workouts)
                    .HasForeignKey(x => x.SplitId)
                    .OnDelete(DeleteBehavior.Cascade);

                _ = entity.HasOne(sw => sw.Workout)
                    .WithMany()
                    .HasForeignKey(x => x.WorkoutId)
                    .OnDelete(DeleteBehavior.Cascade);

                _ = entity.HasIndex(x => x.SplitId);
            });

            _ = modelBuilder.Entity<User>(user =>
            {
                _ = user.HasKey(u => u.Id);

                _ = user.HasOne(u => u.CurrentSplit)
                    .WithMany()
                    .HasForeignKey(u => u.SplitId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);

                _ = user.HasIndex(x => x.Email).IsUnique();
                _ = user.HasIndex(x => x.Username).IsUnique();
            });

            _ = modelBuilder.Entity<UserSettings>(userSettings =>
            {
                _ = userSettings.HasKey(x => x.UserId);

                _ = userSettings.HasOne(x => x.User)
                    .WithOne(x => x.Settings)
                    .HasForeignKey<UserSettings>(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                _ = userSettings.HasIndex(x => x.UserId);
            });

            _ = modelBuilder.Entity<UserFollows>(entity =>
            {
                _ = entity.HasKey(e => new { e.FollowerId, e.FolloweeId });

                _ = entity.HasOne(e => e.Follower)
                      .WithMany(u => u.Following)
                      .HasForeignKey(e => e.FollowerId)
                      .OnDelete(DeleteBehavior.NoAction);

                _ = entity.HasOne(e => e.Followee)
                      .WithMany(u => u.Followers)
                      .HasForeignKey(e => e.FolloweeId)
                      .OnDelete(DeleteBehavior.NoAction);

                _ = entity.HasIndex(e => e.FollowerId);
                _ = entity.HasIndex(e => e.FolloweeId);
                _ = entity.HasIndex(e => new { e.FollowerId, e.FolloweeId }).IsUnique();
            });

            _ = modelBuilder.Entity<Workout>(workout =>
                {
                    _ = workout.HasKey(w => w.Id);

                    _ = workout.HasOne(w => w.Creator)
                        .WithMany(u => u.CreatedWorkouts)
                        .HasForeignKey(u => u.CreatorId)
                        .OnDelete(DeleteBehavior.Cascade);

                    _ = workout.HasMany(w => w.Sets)
                        .WithOne()
                        .HasForeignKey(s => s.WorkoutId)
                        .OnDelete(DeleteBehavior.Cascade);

                    _ = workout.HasMany(w => w.Likes)
                        .WithMany()
                        .UsingEntity<WorkoutLike>(j =>
                        {
                            _ = j.HasOne(x => x.Workout).WithMany().HasForeignKey(x => x.WorkoutId).OnDelete(DeleteBehavior.NoAction);
                            _ = j.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
                            _ = j.HasKey(x => new { x.WorkoutId, x.UserId });
                            _ = j.HasIndex(x => x.WorkoutId);
                            _ = j.HasIndex(x => new { x.WorkoutId, x.UserId }).IsUnique();
                        });

                    _ = workout.HasMany(w => w.Comments)
                        .WithOne()
                        .HasForeignKey(c => c.WorkoutId)
                        .OnDelete(DeleteBehavior.NoAction);

                    _ = workout.HasMany(w => w.Favorites)
                        .WithMany()
                        .UsingEntity<FavoriteWorkout>(j =>
                        {
                            _ = j.HasOne(x => x.Workout).WithMany().HasForeignKey(x => x.WorkoutId).OnDelete(DeleteBehavior.NoAction);
                            _ = j.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
                            _ = j.HasKey(x => new { x.WorkoutId, x.UserId });
                            _ = j.HasIndex(x => x.WorkoutId);
                            _ = j.HasIndex(x => new { x.WorkoutId, x.UserId }).IsUnique();
                        });

                    _ = workout.HasIndex(x => x.CreatorId);
                    _ = workout.HasIndex(x => x.IsPublic);
                });

            _ = modelBuilder.Entity<WorkoutComment>(workoutComment =>
            {
                _ = workoutComment.HasKey(wc => wc.Id);

                _ = workoutComment.HasOne(wc => wc.Creator)
                    .WithMany()
                    .HasForeignKey(wc => wc.CreatorId)
                    .OnDelete(DeleteBehavior.Cascade);

                _ = workoutComment.HasMany(wc => wc.Children)
                    .WithOne()
                    .IsRequired(false)
                    .HasForeignKey(wc => wc.ParentId)
                    .OnDelete(DeleteBehavior.NoAction);

                _ = workoutComment.HasMany(wc => wc.Likes)
                    .WithMany()
                    .UsingEntity<WorkoutCommentLike>(j =>
                    {
                        _ = j.HasOne<WorkoutComment>().WithMany().HasForeignKey(x => x.WorkoutCommentId).OnDelete(DeleteBehavior.NoAction);
                        _ = j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
                        _ = j.HasOne<Workout>().WithMany().HasForeignKey(x => x.WorkoutId).OnDelete(DeleteBehavior.NoAction);
                        _ = j.HasKey(x => new { x.WorkoutCommentId, x.UserId });
                        _ = j.HasIndex(x => x.WorkoutCommentId);
                        _ = j.HasIndex(x => new { x.WorkoutCommentId, x.UserId }).IsUnique();
                    });

                _ = workoutComment.HasIndex(wc => wc.WorkoutId);
            });
        }
    }
}
