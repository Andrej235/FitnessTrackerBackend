namespace FitnessTracker.DTOs.Requests.Split
{
    public class UpdateSplitBaseInfoRequestDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsPublic { get; set; }
    }
}
