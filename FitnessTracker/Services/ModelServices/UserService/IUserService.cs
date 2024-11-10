using FitnessTracker.DTOs.Requests.Completed;
using FitnessTracker.DTOs.Requests.Pins;
using FitnessTracker.DTOs.Requests.User;
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
        Task ConfirmEmail(Guid code, Guid userId);
        Task CreatePins(Guid userId, CreatePinsRequestDTO request);
        Task DeletePins(Guid userId, DeletePinsRequestDTO request);
        Task Follow(Guid userId, string usernameToFollow);
        Task Unfollow(Guid userId, string usernameToFollow);
        Task SendForgotPasswordEmail(SendForgotPasswordEmailRequestDTO request);
        Task ConfirmForgotPasswordEmail(Guid code, ResetPasswordUserRequestDTO request);
        Task<IEnumerable<PinResponseDTO>> GetPinOptions(Guid userId, int? offset, int? limit);
        Task MarkCompletedWorkout(Guid userId, CreateCompletedWorkoutRequestDTO request);
        Task<SimpleUserResponseDTO> GetBasicInfo(Guid userId);
        Task<DetailedUserResponseDTO> GetDetailed(string username, Guid? userId);
        Task<IEnumerable<SimpleUserResponseDTO>> GetFollowingFor(string username, string? nameFilter, int? offset, int? limit);
        Task<IEnumerable<SimpleUserResponseDTO>> GetFollowersFor(string username, string? nameFilter, int? offset, int? limit);
        Task<IEnumerable<PinResponseDTO>> GetPinsFor(string username);
        Task<UserSettingsResponseDTO> GetSettings(Guid userId);
        Task<IEnumerable<SimpleWeekOfCompletedWorkoutsResponseDTO>> GetStreak(string username, int? year);
        Task<DetailedWeekOfCompletedWorkoutsResponseDTO> GetUserStreakOnWeek(string username, DateTime date);
        Task<DetailedWorkoutResponseDTO> GetTodaysWorkout(Guid userId);
        Task ReorderPins(Guid userId, ReorderPinsRequestDTO request);
        Task Update(Guid userId, UpdatePasswordUserRequestDTO request);
        Task Update(Guid userId, UpdateUserEmailRequestDTO request);
        Task Update(Guid userId, UpdateSplitUserRequestDTO request);
        Task Update(Guid userId, UpdateUserNameRequestDTO request);
        Task Update(Guid userId, UpdateUserBioRequestDTO request);
        Task Update(Guid userId, UpdateUserImageRequestDTO request);
        Task UpdateSettings(Guid userId, UpdateUserSettingsRequestDTO request);
    }
}
