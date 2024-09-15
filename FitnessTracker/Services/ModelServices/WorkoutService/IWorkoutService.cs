using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.DTOs.Responses.Workout;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public interface IWorkoutService
    {
        Task<NewWorkoutResponseDTO> Create(Guid userId, CreateWorkoutRequestDTO request);
        Task<string> CreateComment(Guid userId, Guid workoutId, CreateWorkoutCommentRequestDTO request);
        Task<string> CreateReply(Guid userId, Guid workoutId, Guid commentId, CreateWorkoutCommentRequestDTO request);
        Task Delete(Guid userId, Guid id);
        Task DeleteComment(Guid userId, Guid workoutId, Guid commentId);
        Task CreateFavorite(Guid userId, Guid workoutId);
        Task DeleteFavorite(Guid userId, Guid workoutId);
        Task CreateLike(Guid userId, Guid workoutId);
        Task DeleteLike(Guid userId, Guid workoutId);
        Task CreateCommentLike(Guid userId, Guid commentId, Guid workoutId);
        Task DeleteCommentLike(Guid userId, Guid commentId);
        Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAll(string? name, int? limit, int? offset);
        Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAllBy(string username, string? nameFilter, int? limit, int? offset);
        Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAllPersonal(Guid userId, string? nameFilter, int? limit, int? offset);
        Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAllFavorites(Guid userId, string? nameFilter, int? limit, int? offset);
        Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAllLiked(Guid userId, string? nameFilter, int? limit, int? offset);
        Task<DetailedWorkoutResponseDTO> GetDetailed(Guid workoutId, Guid? userId);
        Task<IEnumerable<SimpleWorkoutCommentResponseDTO>> GetComments(Guid workoutId, Guid? userId, int? offset, int? limit);
        Task<IEnumerable<SimpleWorkoutCommentResponseDTO>> GetReplies(Guid workoutId, Guid commentId, Guid? userId, int? offset, int? limit);
        Task UpdateWorkout(Guid userId, Guid workoutId, UpdateFullWorkoutRequestDTO request);
    }
}
