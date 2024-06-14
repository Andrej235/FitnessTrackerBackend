namespace FitnessTracker.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;

        public IEnumerable<MuscleGroup> PrimaryMuscleGroups { get; set; } = new HashSet<MuscleGroup>();
        public IEnumerable<MuscleGroup> SecondaryMuscleGroups { get; set; } = new HashSet<MuscleGroup>();
        public IEnumerable<Muscle> PrimaryMuscles { get; set; } = new HashSet<Muscle>();
        public IEnumerable<Muscle> SecondaryMuscles { get; set; } = new HashSet<Muscle>();
        public IEnumerable<Equipment> Equipment { get; set; } = new HashSet<Equipment>();
        public IEnumerable<User> Favorites { get; set; } = new HashSet<User>();
    }
}