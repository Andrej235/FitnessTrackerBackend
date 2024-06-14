using FitnessTracker.Models;

namespace FitnessTracker.Auth
{
    public interface ITokenManager
    {
        Task<string> CreateJWTAndRefreshToken(User user, IResponseCookies cookies);
        Task<string> RefreshJWT(Guid jwtId, Guid refreshToken, Guid userId);
        Task InvalidateAllTokensForUser(Guid userId);
        Task InvalidateRefreshToken(Guid refreshToken);
    }
}
