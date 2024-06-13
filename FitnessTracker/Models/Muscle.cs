﻿namespace FitnessTracker.Models
{
    public class Muscle
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public IEnumerable<Exercise> PrimaryInExercises { get; set; } = new List<Exercise>();
        public IEnumerable<Exercise> SecondaryInExercises { get; set; } = new List<Exercise>();

        public int MuscleGroupId { get; set; }
    }
}