using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.Models;

namespace FitnessTracker.Data
{
    public class ExerciseContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<MuscleGroup> MuscleGroups { get; set; }
        public DbSet<Muscle> Muscles { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<PrimaryMuscleGroupInExercise> PrimaryMuscleGroups { get; set; }
        public DbSet<SecondaryMuscleGroupInExercise> SecondaryMuscleGroups { get; set; }
        public DbSet<PrimaryMuscleInExercise> PrimaryMuscles { get; set; }
        public DbSet<SecondaryMuscleInExercise> SecondaryMuscles { get; set; }
        public DbSet<EquipmentUsage> EquipmentUsages { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<Workout> Workouts { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FitnessTracker;Integrated Security=True;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Exercise
            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.PrimaryMuscleGroups)
                .WithMany(m => m.PrimaryInExercises)
                .UsingEntity<PrimaryMuscleGroupInExercise>(
                    j => j.HasOne<MuscleGroup>().WithMany().HasForeignKey(m => m.MuscleGroupId).OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Exercise>().WithMany().HasForeignKey(e => e.ExerciseId).OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.Property(me => me.Id).ValueGeneratedOnAdd();
                        j.HasKey(me => me.Id);
                    }
                );

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.SecondaryMuscleGroups)
                .WithMany(m => m.SecondaryInExercises)
                .UsingEntity<SecondaryMuscleGroupInExercise>(
                    j => j.HasOne<MuscleGroup>().WithMany().HasForeignKey(m => m.MuscleGroupId).OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Exercise>().WithMany().HasForeignKey(e => e.ExerciseId).OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.Property(me => me.Id).ValueGeneratedOnAdd();
                        j.HasKey(me => me.Id);
                    }
                );

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.PrimaryMuscles)
                .WithMany(m => m.PrimaryInExercises)
                .UsingEntity<PrimaryMuscleInExercise>(
                    j => j.HasOne<Muscle>().WithMany().HasForeignKey(m => m.MuscleId).OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Exercise>().WithMany().HasForeignKey(e => e.ExerciseId).OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.Property(me => me.Id).ValueGeneratedOnAdd();
                        j.HasKey(me => me.Id);
                    }
                );

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.SecondaryMuscles)
                .WithMany(m => m.SecondaryInExercises)
                .UsingEntity<SecondaryMuscleInExercise>(
                    j => j.HasOne<Muscle>().WithMany().HasForeignKey(m => m.MuscleId).OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Exercise>().WithMany().HasForeignKey(e => e.ExerciseId).OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.Property(me => me.Id).ValueGeneratedOnAdd();
                        j.HasKey(me => me.Id);
                    }
                );

            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.Equipment)
                .WithMany(e => e.UsedInExercises)
                .UsingEntity<EquipmentUsage>(
                    j => j.HasOne<Equipment>().WithMany().HasForeignKey(e => e.EquipmentId).OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Exercise>().WithMany().HasForeignKey(e => e.ExerciseId).OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.Property(ee => ee.Id).ValueGeneratedOnAdd();
                        j.HasKey(ee => ee.Id);
                    }
                );
            #endregion

            #region Workouts
            modelBuilder.Entity<Workout>()
                .HasMany(w => w.Sets)
                .WithOne()
                .HasForeignKey(ws => ws.WorkoutId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Set>()
                .HasOne(s => s.Exercise)
                .WithMany()
                .HasForeignKey(s => s.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            modelBuilder.Entity<RefreshToken>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
