using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.DTOs.Responses.Workout;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public interface IWorkoutService
    {
        Task<NewWorkoutResponseDTO> Create(Guid userId, CreateWorkoutRequestDTO request);
        Task<Guid> CreateComment(Guid userId, Guid workoutId, CreateWorkoutCommentRequestDTO request);
        Task<Guid> CreateReply(Guid userId, Guid workoutId, Guid commentId, CreateWorkoutCommentRequestDTO request);
        Task Delete(Guid userId, string creatorUsername, string workoutName);
        Task DeleteComment(Guid userId, Guid workoutId, Guid commentId);
        Task CreateFavorite(Guid userId, Guid workoutId);
        Task DeleteFavorite(Guid userId, Guid workoutId);
        Task CreateLike(Guid userId, Guid workoutId);
        Task DeleteLike(Guid userId, Guid workoutId);
        Task CreateCommentLike(Guid userId, Guid commentId, Guid workoutId);
        Task DeleteCommentLike(Guid userId, Guid commentId);
        Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAllBy(string username, string? nameFilter, int? limit, int? offset);
        Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAllFavoritesBy(string username, Guid? userId, string? nameFilter, int? limit, int? offset);
        Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAllLikedBy(string username, Guid? userId, string? nameFilter, int? limit, int? offset);
        Task<DetailedWorkoutResponseDTO> GetDetailed(string creatorUsername, string workoutName, Guid? userId);
        Task<IEnumerable<SimpleWorkoutCommentResponseDTO>> GetComments(Guid workoutId, Guid? userId, int? offset, int? limit);
        Task<IEnumerable<SimpleWorkoutCommentResponseDTO>> GetReplies(Guid workoutId, Guid commentId, Guid? userId, int? offset, int? limit);
        Task UpdateWorkout(Guid userId, string creatorUsername, string workoutName, UpdateFullWorkoutRequestDTO request);
        Task<WorkoutExerciseChartDataResponseDTO> GetChartDataForExercise(Guid userId, string username, string workoutName, int exerciseId, DateTime? startDate);
        Task<WorkoutExerciseChartDataResponseDTO> MockChartDataForExercise(Guid userId, string username, string workoutName, int exerciseId, DateTime? startDate);
    }
}
