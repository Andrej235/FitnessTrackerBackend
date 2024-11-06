using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.DTOs.Responses.Workout;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Services.ModelServices.SplitService
{
    public interface ISplitService
    {
        Task UpdateBasicInformation(Guid splitId, Guid userId, UpdateSplitBaseInfoRequestDTO request);
        Task UpdateSplitWorkout(Guid splitId, Guid userId, DayOfWeek day, [FromBody] UpdateSplitWorkoutRequestDTO request);
        Task<IEnumerable<SimpleSplitCommentResponseDTO>> GetComments(Guid splitId, Guid? userId, int? offset, int? limit);
        Task<IEnumerable<SimpleSplitCommentResponseDTO>> GetReplies(Guid splitId, Guid commentId, Guid? userId, int? offset, int? limit);
        Task<IEnumerable<SimpleSplitResponseDTO>> GetAllPublicBy(string username, string? splitNameFilter, int? offset, int? limit);
        Task<IEnumerable<SimpleSplitResponseDTO>> GetAllPersonal(Guid userId, string? splitNameFilter, int? offset, int? limit);
        Task<IEnumerable<SimpleSplitResponseDTO>> GetAllFavorites(Guid userId, string? nameFilter, int? limit, int? offset);
        Task<IEnumerable<SimpleSplitResponseDTO>> GetAllLiked(Guid userId, string? nameFilter, int? limit, int? offset);
        Task<DetailedSplitResponseDTO> GetSingleDetailed(Guid splitId, Guid? userId);
        Task<DetailedUserSplitResponseDTO> GetDetailedUsedBy(string username);
        Task<IEnumerable<SimpleWorkoutOptionResponseDTO>> GetSplitWorkoutOptions(Guid userId, int? offset, int? limit, string? nameFilter, bool? publicOnly, bool? favoritesOnly, bool? personalOnly);
        Task CreateCommentLike(Guid commentId, Guid userId);
        Task DeleteCommentLike(Guid commentId, Guid userId);
        Task CreateLike(Guid splitId, Guid userId);
        Task DeleteLike(Guid splitId, Guid userId);
        Task CreateFavorite(Guid splitId, Guid userId);
        Task DeleteFavorite(Guid splitId, Guid userId);
        Task DeleteComment(Guid splitId, Guid commentId, Guid userId);
        Task<Guid> CreateComment(Guid splitId, Guid userId, CreateSplitCommentRequestDTO request);
        Task<Guid> CreateReply(Guid splitId, Guid commentId, Guid userId, CreateSplitCommentRequestDTO request);
        Task<NewSplitResponseDTO> Create(Guid userId, CreateSplitRequestDTO request);
    }
}
