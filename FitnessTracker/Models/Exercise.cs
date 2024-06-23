namespace FitnessTracker.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;

        public ICollection<MuscleGroup> PrimaryMuscleGroups { get; set; } = [];
        public ICollection<MuscleGroup> SecondaryMuscleGroups { get; set; } = [];
        public ICollection<Muscle> PrimaryMuscles { get; set; } = [];
        public ICollection<Muscle> SecondaryMuscles { get; set; } = [];
        public ICollection<Equipment> Equipment { get; set; } = [];
        public ICollection<User> Favorites { get; set; } = [];
    }
}