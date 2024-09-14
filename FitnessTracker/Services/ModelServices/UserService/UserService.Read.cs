using FitnessTracker.DTOs.Responses.User;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public Task<DetailedUserResponseDTO> GetDetailed(Guid userId) => throw new NotImplementedException();
        public Task<DetailedPublicUserResponseDTO> GetDetailed(Guid userId, string username) => throw new NotImplementedException();
        public Task<UserProfilePictureResponseDTO> GetProfilePic(Guid userId) => throw new NotImplementedException();
        public Task<IEnumerable<SimpleUserResponseDTO>> GetFollowers(Guid userId, string? nameFilter, int? limit, int? offset) => throw new NotImplementedException();
        public Task<IEnumerable<SimpleUserResponseDTO>> GetFollowersFor(string username, string? nameFilter, int? limit, int? offset) => throw new NotImplementedException();
        public Task<IEnumerable<SimpleUserResponseDTO>> GetFollowing(Guid userId, string? nameFilter, int? limit, int? offset) => throw new NotImplementedException();
        public Task<IEnumerable<SimpleUserResponseDTO>> GetFollowingFor(string username, string? nameFilter, int? limit, int? offset) => throw new NotImplementedException();
    }
}
