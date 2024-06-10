using ProjectGym.Services.DatabaseSerialization;
using System.ComponentModel.DataAnnotations;

namespace ProjectGym.Models
{
    public class Set
    {
        [Key]
        public Guid Id { get; set; }
        public int BottomRepRange { get; set; }
        public int TopRepRange { get; set; }
        public int NumberOfSets { get; set; }

        public Exercise Exercise { get; set; } = null!;
        [ModelReference("Exercise")]
        public int ExerciseId { get; set; }

        [ModelReference("Workout")]
        public Guid WorkoutId { get; set; }
    }
}
