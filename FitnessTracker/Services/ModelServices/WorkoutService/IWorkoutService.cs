using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.DTOs.Responses.Workout;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public interface IWorkoutService
    {
        Task<NewWorkoutResponseDTO> Create(CreateWorkoutRequestDTO request);
        Task<string> CreateComment(Guid workoutId, CreateWorkoutCommentRequestDTO request);
        Task<string> CreateReply(Guid workoutId, Guid commentId, CreateWorkoutCommentRequestDTO request);
        Task Delete(Guid id);
        Task DeleteComment(Guid workoutId, Guid commentId);
        Task CreateFavorite(Guid id);
        Task DeleteFavorite(Guid id);
        Task CreateLike(Guid id);
        Task DeleteLike(Guid id);
        Task CreateCommentLike(Guid id, Guid workoutId);
        Task DeleteCommentLike(Guid id);
        Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAll(string? name, int? limit, int? offset);
        Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAllBy(string username, string? nameFilter, int? limit, int? offset);
        Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAllPersonal(Guid userId, string? name, int? limit, int? offset);
        Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAllFavorites(string? nameFilter, int? limit, int? offset);
        Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAllLiked(string? nameFilter, int? limit, int? offset);
        Task<DetailedWorkoutResponseDTO> GetDetailed(Guid id);
        Task<IEnumerable<SimpleWorkoutCommentResponseDTO>> GetComments(Guid workoutId, int? offset, int? limit);
        Task<IEnumerable<SimpleWorkoutCommentResponseDTO>> GetReplies(Guid workoutId, Guid commentId, int? offset, int? limit);
        Task UpdateWorkout(Guid id, UpdateFullWorkoutRequestDTO request);
    }
}
