using FitnessTracker.DTOs.Requests.Exercise;
using FitnessTracker.DTOs.Responses.Exercises;

namespace FitnessTracker.Services.ModelServices.ExerciseService
{
    public interface IExerciseService
    {
        Task Create(CreateExerciseRequestDTO newExercise);
        Task<IEnumerable<SimpleExerciseResponseDTO>> GetAll(int? muscleGroupId, int? equipmentId, string? name, int? offset, int? limit);
        Task<DetailedExerciseResponseDTO> GetDetailed(int exerciseId, Guid? userId);
        Task Update(UpdateExerciseRequestDTO updatedExercise);
        Task Delete(int id);

        Task CreateFavorite(int exerciseId, Guid userId);
        Task DeleteFavorite(int exerciseId, Guid userId);

        Task<IEnumerable<ExerciseHistorySegmentResponseDTO>> GetHistory(int exerciseId, Guid userId, int? offset, int? limit);
    }
}
