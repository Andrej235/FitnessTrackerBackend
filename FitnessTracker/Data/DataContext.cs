using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompletedWorkout>(completedWorkout =>
            {
                completedWorkout.HasKey(c => c.Id);

                completedWorkout.HasOne(cw => cw.Workout)
                    .WithMany()
                    .HasForeignKey(cw => cw.WorkoutId)
                    .OnDelete(DeleteBehavior.NoAction);

                completedWorkout.HasOne<User>()
                    .WithMany(cw => cw.CompletedWorkouts)
                    .HasForeignKey(cw => cw.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                completedWorkout.HasOne(cw => cw.Split)
                    .WithMany()
                    .HasForeignKey(cw => cw.SplitId)
                    .OnDelete(DeleteBehavior.NoAction);

                completedWorkout.HasIndex(x => x.UserId);
                completedWorkout.HasIndex(x => new { x.UserId, x.WorkoutId });
            });

            modelBuilder.Entity<EmailConfirmation>(emailConformation =>
            {
                emailConformation.HasKey(e => e.Id);

                emailConformation.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                emailConformation.HasIndex(x => x.Id).IsUnique();
                emailConformation.HasIndex(x => x.UserId);
            });

            modelBuilder.Entity<Equipment>(equipment =>
            {
                equipment.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Exercise>(exercise =>
            {
                exercise.HasKey(e => e.Id);

                exercise.HasMany(e => e.PrimaryMuscles)
                    .WithMany(m => m.PrimaryInExercises)
                    .UsingEntity<PrimaryMuscleInExercise>(j =>
                    {
                        j.HasOne<Exercise>().WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
                        j.HasOne<Muscle>().WithMany().HasForeignKey(x => x.MuscleId).OnDelete(DeleteBehavior.Cascade);
                        j.HasKey(x => new { x.ExerciseId, x.MuscleId });
                        j.HasIndex(x => x.ExerciseId);
                    });

                exercise.HasMany(e => e.PrimaryMuscleGroups)
                    .WithMany(m => m.PrimaryInExercises)
                    .UsingEntity<PrimaryMuscleGroupInExercise>(j =>
                    {
                        j.HasOne<Exercise>().WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
                        j.HasOne<MuscleGroup>().WithMany().HasForeignKey(x => x.MuscleGroupId).OnDelete(DeleteBehavior.Cascade);
                        j.HasKey(x => new { x.ExerciseId, x.MuscleGroupId });
                        j.HasIndex(x => x.ExerciseId);
                    });

                exercise.HasMany(e => e.SecondaryMuscles)
                    .WithMany(m => m.SecondaryInExercises)
                    .UsingEntity<SecondaryMuscleInExercise>(j =>
                    {
                        j.HasOne<Exercise>().WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
                        j.HasOne<Muscle>().WithMany().HasForeignKey(x => x.MuscleId).OnDelete(DeleteBehavior.Cascade);
                        j.HasKey(x => new { x.ExerciseId, x.MuscleId });
                        j.HasIndex(x => x.ExerciseId);
                    });

                exercise.HasMany(e => e.SecondaryMuscleGroups)
                    .WithMany(m => m.SecondaryInExercises)
                    .UsingEntity<SecondaryMuscleGroupInExercise>(j =>
                    {
                        j.HasOne<Exercise>().WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
                        j.HasOne<MuscleGroup>().WithMany().HasForeignKey(x => x.MuscleGroupId).OnDelete(DeleteBehavior.Cascade);
                        j.HasKey(x => new { x.ExerciseId, x.MuscleGroupId });
                        j.HasIndex(x => x.ExerciseId);
                    });

                exercise.HasMany(e => e.Equipment)
                    .WithMany(eq => eq.UsedInExercises)
                    .UsingEntity<EquipmentUsage>(j =>
                    {
                        j.HasOne<Exercise>().WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
                        j.HasOne<Equipment>().WithMany().HasForeignKey(x => x.EquipmentId).OnDelete(DeleteBehavior.Cascade);
                        j.HasKey(x => new { x.ExerciseId, x.EquipmentId });
                        j.HasIndex(x => x.ExerciseId);
                    });

                exercise.HasMany(e => e.Favorites)
                    .WithMany()
                    .UsingEntity<FavoriteExercise>(j =>
                    {
                        j.HasOne<Exercise>().WithMany().HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Cascade);
                        j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                        j.HasKey(x => new { x.ExerciseId, x.UserId });
                        j.HasIndex(x => x.ExerciseId);
                        j.HasIndex(x => new { x.ExerciseId, x.UserId }).IsUnique();
                    });
            });

            modelBuilder.Entity<Muscle>(muscle =>
            {
                muscle.HasKey(m => m.Id);

                muscle.HasOne(m => m.MuscleGroup)
                    .WithMany(mg => mg.Muscles)
                    .HasForeignKey(m => m.MuscleGroupId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MuscleGroup>(muscleGroup =>
            {
                muscleGroup.HasKey(mg => mg.Id);
            });

            modelBuilder.Entity<Post>(post =>
            {
                post.HasKey(p => p.Id);

                post.HasOne(p => p.Creator)
                    .WithMany()
                    .HasForeignKey(p => p.CreatorId)
                    .OnDelete(DeleteBehavior.NoAction);

                post.HasMany(p => p.Likes)
                    .WithMany()
                    .UsingEntity<PostLike>(j =>
                    {
                        j.HasOne<Post>().WithMany().HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.Cascade);
                        j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                        j.HasKey(x => new { x.PostId, x.UserId });
                        j.HasIndex(x => x.PostId);
                        j.HasIndex(x => new { x.PostId, x.UserId }).IsUnique();
                    });

                post.HasMany(p => p.Comments)
                    .WithOne()
                    .HasForeignKey(p => p.PostId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PostComment>(postComment =>
            {
                postComment.HasKey(pc => pc.Id);

                postComment.HasOne(pc => pc.Creator)
                    .WithMany()
                    .HasForeignKey(pc => pc.CreatorId)
                    .OnDelete(DeleteBehavior.Cascade);

                postComment.HasMany(pc => pc.Children)
                    .WithOne()
                    .IsRequired(false)
                    .HasForeignKey(pc => pc.ParentId)
                    .OnDelete(DeleteBehavior.NoAction);

                postComment.HasMany(pc => pc.Likes)
                    .WithMany()
                    .UsingEntity<PostCommentLike>(j =>
                    {
                        j.HasOne<PostComment>().WithMany().HasForeignKey(x => x.PostCommentId).OnDelete(DeleteBehavior.NoAction);
                        j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
                        j.HasKey(x => new { x.PostCommentId, x.UserId });
                        j.HasIndex(x => x.PostCommentId);
                        j.HasIndex(x => new { x.PostCommentId, x.UserId }).IsUnique();
                    });

                postComment.HasIndex(pc => pc.CreatorId);
            });

            modelBuilder.Entity<RefreshToken>(refreshToken =>
            {
                refreshToken.HasKey(p => p.Token);

                refreshToken.HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                refreshToken.HasIndex(x => x.UserId);
            });

            modelBuilder.Entity<Set>(set =>
            {
                set.HasKey(x => x.Id);

                set.HasOne(s => s.Exercise)
                    .WithMany()
                    .HasForeignKey(s => s.ExerciseId)
                    .OnDelete(DeleteBehavior.Cascade);

                set.HasOne<Workout>()
                    .WithMany(w => w.Sets)
                    .HasForeignKey(s => s.WorkoutId)
                    .OnDelete(DeleteBehavior.Cascade);

                set.HasIndex(x => x.WorkoutId);
            });

            modelBuilder.Entity<Split>(split =>
            {
                split.HasKey(s => s.Id);

                split.HasOne(s => s.Creator)
                    .WithMany(u => u.CreatedSplits)
                    .HasForeignKey(s => s.CreatorId)
                    .OnDelete(DeleteBehavior.NoAction);

                split.HasMany(s => s.Likes)
                    .WithMany()
                    .UsingEntity<SplitLike>(j =>
                    {
                        j.HasOne<Split>().WithMany().HasForeignKey(x => x.SplitId).OnDelete(DeleteBehavior.Cascade);
                        j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                        j.HasKey(x => new { x.SplitId, x.UserId });
                        j.HasIndex(x => x.SplitId);
                        j.HasIndex(x => new { x.SplitId, x.UserId }).IsUnique();
                    });

                split.HasMany(s => s.Comments)
                    .WithOne()
                    .HasForeignKey(s => s.SplitId)
                    .OnDelete(DeleteBehavior.Cascade);

                split.HasMany(s => s.Workouts)
                    .WithOne(sw => sw.Split)
                    .HasForeignKey(sw => sw.SplitId)
                    .OnDelete(DeleteBehavior.Cascade);

                split.HasMany(s => s.Favorites)
                    .WithMany()
                    .UsingEntity<FavoriteSplit>(j =>
                    {
                        j.HasOne<Split>().WithMany().HasForeignKey(x => x.SplitId).OnDelete(DeleteBehavior.Cascade);
                        j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                        j.HasKey(x => new { x.SplitId, x.UserId });
                        j.HasIndex(x => x.SplitId);
                        j.HasIndex(x => new { x.SplitId, x.UserId }).IsUnique();
                    });
            });

            modelBuilder.Entity<SplitComment>(splitComment =>
            {
                splitComment.HasKey(sc => sc.Id);

                splitComment.HasOne(sc => sc.Creator)
                    .WithMany()
                    .HasForeignKey(sc => sc.CreatorId)
                    .OnDelete(DeleteBehavior.Cascade);

                splitComment.HasMany(sc => sc.Children)
                    .WithOne()
                    .IsRequired(false)
                    .HasForeignKey(sc => sc.ParentId)
                    .OnDelete(DeleteBehavior.NoAction);

                splitComment.HasMany(sc => sc.Likes)
                    .WithMany()
                    .UsingEntity<SplitCommentLike>(j =>
                    {
                        j.HasOne<SplitComment>().WithMany().HasForeignKey(x => x.SplitCommentId).OnDelete(DeleteBehavior.NoAction);
                        j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
                        j.HasKey(x => new { x.SplitCommentId, x.UserId });
                        j.HasIndex(x => x.SplitCommentId);
                        j.HasIndex(x => new { x.SplitCommentId, x.UserId }).IsUnique();
                    });

                splitComment.HasIndex(sc => sc.SplitId);
            });

            modelBuilder.Entity<SplitWorkout>(entity =>
            {
                entity.HasKey(x => new { x.SplitId, x.Day });

                entity.HasOne(sw => sw.Split)
                    .WithMany(s => s.Workouts)
                    .HasForeignKey(x => x.SplitId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sw => sw.Workout)
                    .WithMany()
                    .HasForeignKey(x => x.WorkoutId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => x.SplitId);
            });

            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(u => u.Id);

                user.HasOne(u => u.CurrentSplit)
                    .WithMany()
                    .HasForeignKey(u => u.SplitId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);

                user.HasMany(u => u.Followers)
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
            });

            modelBuilder.Entity<Workout>(workout =>
            {
                workout.HasKey(w => w.Id);

                workout.HasOne(w => w.Creator)
                    .WithMany(u => u.CreatedWorkouts)
                    .HasForeignKey(u => u.CreatorId)
                    .OnDelete(DeleteBehavior.Cascade);

                workout.HasMany(w => w.Sets)
                    .WithOne()
                    .HasForeignKey(s => s.WorkoutId)
                    .OnDelete(DeleteBehavior.Cascade);

                workout.HasMany(w => w.Likes)
                    .WithMany()
                    .UsingEntity<WorkoutLike>(j =>
                    {
                        j.HasOne<Workout>().WithMany().HasForeignKey(x => x.WorkoutId).OnDelete(DeleteBehavior.NoAction);
                        j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
                        j.HasKey(x => new { x.WorkoutId, x.UserId });
                        j.HasIndex(x => x.WorkoutId);
                        j.HasIndex(x => new { x.WorkoutId, x.UserId }).IsUnique();
                    });

                workout.HasMany(w => w.Comments)
                    .WithOne()
                    .HasForeignKey(c => c.WorkoutId)
                    .OnDelete(DeleteBehavior.NoAction);

                workout.HasMany(w => w.Favorites)
                    .WithMany()
                    .UsingEntity<FavoriteWorkout>(j =>
                    {
                        j.HasOne<Workout>().WithMany().HasForeignKey(x => x.WorkoutId).OnDelete(DeleteBehavior.NoAction);
                        j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
                        j.HasKey(x => new { x.WorkoutId, x.UserId });
                        j.HasIndex(x => x.WorkoutId);
                        j.HasIndex(x => new { x.WorkoutId, x.UserId }).IsUnique();
                    });

                workout.HasIndex(x => x.CreatorId);
                workout.HasIndex(x => x.IsPublic);
            });

            modelBuilder.Entity<WorkoutComment>(workoutComment =>
            {
                workoutComment.HasKey(wc => wc.Id);

                workoutComment.HasOne(wc => wc.Creator)
                    .WithMany()
                    .HasForeignKey(wc => wc.CreatorId)
                    .OnDelete(DeleteBehavior.Cascade);

                workoutComment.HasMany(wc => wc.Children)
                    .WithOne()
                    .IsRequired(false)
                    .HasForeignKey(wc => wc.ParentId)
                    .OnDelete(DeleteBehavior.NoAction);

                workoutComment.HasMany(wc => wc.Likes)
                    .WithMany()
                    .UsingEntity<WorkoutCommentLike>(j =>
                    {
                        j.HasOne<WorkoutComment>().WithMany().HasForeignKey(x => x.WorkoutCommentId).OnDelete(DeleteBehavior.NoAction);
                        j.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
                        j.HasKey(x => new { x.WorkoutCommentId, x.UserId });
                        j.HasIndex(x => x.WorkoutCommentId);
                        j.HasIndex(x => new { x.WorkoutCommentId, x.UserId }).IsUnique();
                    });

                workoutComment.HasIndex(wc => wc.WorkoutId);
            });
        }
    }
}
