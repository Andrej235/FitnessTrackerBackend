using FitnessTracker.DTOs.Responses.Workout;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService
    {
        public Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAll(string? name, int? limit, int? offset) => throw new NotImplementedException();
        public Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAllBy(string username, string? nameFilter, int? limit, int? offset) => throw new NotImplementedException();
        public Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAllFavorites(string? nameFilter, int? limit, int? offset) => throw new NotImplementedException();
        public Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAllLiked(string? nameFilter, int? limit, int? offset) => throw new NotImplementedException();
        public Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAllPersonal(Guid userId, string? name, int? limit, int? offset) => throw new NotImplementedException();
        public Task<DetailedWorkoutResponseDTO> GetDetailed(Guid id) => throw new NotImplementedException();
    }
}
