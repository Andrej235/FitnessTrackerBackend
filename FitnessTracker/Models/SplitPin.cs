namespace FitnessTracker.Models
{
    public class SplitPin
    {
        public Guid SplitId { get; set; }
        public Split Split { get; set; } = null!;

        public Guid UserId { get; set; }
    }
}
