using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.DTOs.Responses.AuthTokens;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public Task<SimpleJWTResponseDTO> Register(RegisterUserRequestDTO request, IResponseCookies cookies) => throw new NotImplementedException();
        public Task<SimpleJWTResponseDTO> Login(LoginUserRequestDTO request, IResponseCookies cookies) => throw new NotImplementedException();
        public Task<SimpleJWTResponseDTO> Refresh(Guid jwtId, Guid userId, Guid refreshToken) => throw new NotImplementedException();
        public Task Logout(Guid refreshToken) => throw new NotImplementedException();
    }
}
