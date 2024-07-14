namespace FitnessTracker.Models
{
    public class Set
    {
        public enum SetType
        {
            Normal,
            Warmup,
            Dropset,
            Failure,
        }

        public Guid Id { get; set; }
        public int TopRepRange { get; set; }
        public int BottomRepRange { get; set; }
        public SetType Type { get; set; } = SetType.Normal;

        public Exercise Exercise { get; set; } = null!;
        public int ExerciseId { get; set; }

        public Guid WorkoutId { get; set; }
        public int RiR { get; set; }
    }
}
