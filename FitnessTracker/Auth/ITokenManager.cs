namespace FitnessTracker.Auth
{
    public interface ITokenManager
    {
        Task<string> GenerateJWTAndRefreshToken(Guid userId, string role, IResponseCookies cookies);
        Task<string> RefreshJWT(Guid jwtId, Guid refreshToken, Guid userId);
        Task InvalidateAllTokensForUser(Guid userId);
        Task InvalidateRefreshToken(Guid refreshToken);
    }
}
