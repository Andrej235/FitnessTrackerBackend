namespace FitnessTracker.DTOs.Responses.User
{
    public class DetailedPublicUserResponseDTO : SimpleUserResponseDTO
    {
        public string Bio { get; set; } = null!;
        public bool IsMe { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
        public int TotalCompletedWorkouts { get; set; }
        public bool IsFollowing { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
