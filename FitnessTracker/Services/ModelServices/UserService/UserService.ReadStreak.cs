using FitnessTracker.DTOs.Responses.CompletedWorkouts;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public Task<IEnumerable<SimpleWeekOfCompletedWorkoutsResponseDTO>> GetStreak(Guid userId, int? year) => throw new NotImplementedException();
        public Task<IEnumerable<SimpleWeekOfCompletedWorkoutsResponseDTO>> GetStreak(string username, int? year) => throw new NotImplementedException();
        public Task<DetailedWeekOfCompletedWorkoutsResponseDTO> GetUserStreakOnWeek(Guid userId, DateTime date) => throw new NotImplementedException();
    }
}
