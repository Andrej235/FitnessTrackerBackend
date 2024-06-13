using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Models
{
    public class Set
    {
        [Key]
        public Guid Id { get; set; }
        public int BottomRepRange { get; set; }
        public int TopRepRange { get; set; }
        public int NumberOfSets { get; set; }

        public Exercise Exercise { get; set; } = null!;
        public int ExerciseId { get; set; }

        public Guid WorkoutId { get; set; }
    }
}
