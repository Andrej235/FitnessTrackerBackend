using FitnessTracker.DTOs.Requests.Completed;
using FitnessTracker.DTOs.Requests.Pins;
using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.DTOs.Responses.Activity;
using FitnessTracker.DTOs.Responses.AuthTokens;
using FitnessTracker.DTOs.Responses.CompletedWorkouts;
using FitnessTracker.DTOs.Responses.Pins;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.DTOs.Responses.Workout;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public interface IUserService
    {
        Task<SimpleJWTResponseDTO> Register(RegisterUserRequestDTO request, IResponseCookies cookies);
        Task<SimpleJWTResponseDTO> Login(LoginUserRequestDTO request, IResponseCookies cookies);
        Task<SimpleJWTResponseDTO> Refresh(Guid jwtId, Guid userId, Guid refreshToken);
        Task Logout(Guid refreshToken);
        Task ResendConfirmationEmail(Guid userId);
        Task ConfrimEmail(Guid code, Guid userId);
        Task CreatePin(Guid userId, CreatePinsRequestDTO request);
        Task DeletePin(Guid userId, DeletePinsRequestDTO request);
        Task Follow(Guid userId, Guid userToFollowId);
        Task Unfollow(Guid userId, Guid userToFollowId);
        Task SendForgotPasswordEmail(SendForgotPasswordEmailRequestDTO request);
        Task ConfirmForgotPasswordEmail(Guid code, ResetPasswordUserRequestDTO request);
        Task<IEnumerable<PinResponseDTO>> GetPinOptions(Guid userId);
        Task MarkCompletedWorkout(Guid userId, CreateCompletedWorkoutRequestDTO request);
        Task<DetailedUserResponseDTO> GetDetailed(Guid userId);
        Task<UserProfilePictureResponseDTO> GetProfilePic(Guid userId);
        Task<IEnumerable<SimpleUserResponseDTO>> GetFollowing(Guid userId, string? nameFilter, int? limit, int? offset);
        Task<IEnumerable<SimpleUserResponseDTO>> GetFollowers(Guid userId, string? nameFilter, int? limit, int? offset);
        Task<DetailedPublicUserResponseDTO> GetDetailed(Guid userId, string username);
        Task<IEnumerable<SimpleUserResponseDTO>> GetFollowingFor(string username, string? nameFilter, int? limit, int? offset);
        Task<IEnumerable<SimpleUserResponseDTO>> GetFollowersFor(string username, string? nameFilter, int? limit, int? offset);
        Task<UserActivityResponseDTO> ReadLatestActivity(Guid userId);
        Task<IEnumerable<PinResponseDTO>> GetPins(Guid userId);
        Task<IEnumerable<PinResponseDTO>> GetPinsFor(string username);
        Task<UserSettingsResponseDTO> GetSettings(Guid userId);
        Task<IEnumerable<SimpleWeekOfCompletedWorkoutsResponseDTO>> GetStreak(Guid userId, int? year);
        Task<IEnumerable<SimpleWeekOfCompletedWorkoutsResponseDTO>> GetStreak(string username, int? year);
        Task<DetailedWeekOfCompletedWorkoutsResponseDTO> GetUserStreakOnWeek(Guid userId, DateTime date);
        Task<DetailedWorkoutResponseDTO> GetTodaysWorkout(Guid userId);
        Task ReorderPins(ReorderPinsRequestDTO request);
        Task Update(UpdatePasswordUserRequestDTO request);
        Task Update(UpdateUserEmailRequestDTO request);
        Task Update(UpdateSplitUserRequestDTO request);
        Task Update(UpdateUserNameRequestDTO request);
        Task Update(UpdateUserBioRequestDTO request);
        Task Update(UpdateUserImageRequestDTO request);
        Task UpdateSettings(UpdateUserSettingsRequestDTO request);
    }
}
