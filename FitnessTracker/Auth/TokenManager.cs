using FitnessTracker.Models;
using Microsoft.IdentityModel.Tokens;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitnessTracker.Auth
{
    public class TokenManager(ConfigurationManager configuration,
                              IReadService<RefreshToken> readService,
                              ICreateService<RefreshToken> createService,
                              IUpdateService<RefreshToken> updateService,
                              IDeleteService<RefreshToken> deleteService,
                              IDeleteRangeService<RefreshToken> deleteRangeService) : ITokenManager
    {
        private readonly ConfigurationManager configuration = configuration;
        private readonly IReadService<RefreshToken> readService = readService;
        private readonly ICreateService<RefreshToken> createService = createService;
        private readonly IUpdateService<RefreshToken> updateService = updateService;
        private readonly IDeleteService<RefreshToken> deleteService = deleteService;
        private readonly IDeleteRangeService<RefreshToken> deleteRangeService = deleteRangeService;

        private (string jwt, Guid jwtId) CreateJWTAndId(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
            var jwtId = Guid.NewGuid();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, jwtId.ToString()),
                        new Claim(ClaimTypes.Role, user.Role)
                    ]
                ),

                Expires = DateTime.UtcNow.Add(TimeSpan.Parse(configuration["Jwt:TokenLifespan"]!)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return (tokenString, jwtId);
        }

        public async Task<string> GenerateJWTAndRefreshToken(User user, IResponseCookies cookies)
        {
            var (jwt, jwtId) = CreateJWTAndId(user);

            RefreshToken refresh = new()
            {
                JwtId = jwtId,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
            };

            await createService.Add(refresh);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false, //TODO-PROD: Set to true in production
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7) //TODO-PROD: Set to something concrete in production
            };
            cookies.Append("refreshToken", refresh.Token.ToString(), cookieOptions);

            return jwt;
        }

        public async Task<string> RefreshJWT(Guid jwtId, Guid refreshToken, Guid userId)
        {
            var token = await readService.Get(x => x.Token == refreshToken, "none");
            if (token is null || token.JwtId != jwtId || token.UserId != userId)
                throw new Exception("Invalid token");

            var (newJwt, newJwtId) = CreateJWTAndId(token.User);
            token.JwtId = newJwtId;
            await updateService.Update(token);

            return newJwt;
        }

        public Task InvalidateAllTokensForUser(Guid userId) => deleteRangeService.Delete(x => x.UserId == userId);

        public Task InvalidateRefreshToken(Guid refreshToken) => deleteService.Delete(x => x.Token == refreshToken);
    }
}
