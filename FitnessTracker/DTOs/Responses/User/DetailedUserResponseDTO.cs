using FitnessTracker.DTOs.Responses.Split;

namespace FitnessTracker.DTOs.Responses.User
{
    public class DetailedUserResponseDTO : SimpleUserResponseDTO
    {
        public string Bio { get; set; } = null!;
        public int Followers { get; set; }
        public int Following { get; set; }
        public int TotalCompletedWorkouts { get; set; }
        public DetailedUserSplitResponseDTO? CurrentSplit { get; set; }
        public DateTime JoinedAt { get; set; }
        public Models.Gender Gender { get; set; }
    }
}
