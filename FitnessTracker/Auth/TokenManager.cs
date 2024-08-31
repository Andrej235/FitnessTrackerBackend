using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitnessTracker.Auth
{
    public class TokenManager(ConfigurationManager configuration,
                              IReadSingleService<RefreshToken> readSingleService,
                              ICreateService<RefreshToken> createService,
                              IUpdateService<RefreshToken> updateService,
                              IDeleteService<RefreshToken> deleteService,
                              IDeleteRangeService<RefreshToken> deleteRangeService) : ITokenManager
    {
        private readonly ConfigurationManager configuration = configuration;
        private readonly IReadSingleService<RefreshToken> readSingleService = readSingleService;
        private readonly ICreateService<RefreshToken> createService = createService;
        private readonly IUpdateService<RefreshToken> updateService = updateService;
        private readonly IDeleteService<RefreshToken> deleteService = deleteService;
        private readonly IDeleteRangeService<RefreshToken> deleteRangeService = deleteRangeService;

        private (string jwt, Guid jwtId) CreateJWTAndId(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
            Guid jwtId = Guid.NewGuid();

            SecurityTokenDescriptor tokenDescriptor = new()
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

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            return (tokenString, jwtId);
        }

        public async Task<string> GenerateJWTAndRefreshToken(User user, IResponseCookies cookies)
        {
            (string jwt, Guid jwtId) = CreateJWTAndId(user);

            RefreshToken refresh = new()
            {
                JwtId = jwtId,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
            };

            _ = await createService.Add(refresh);

            CookieOptions cookieOptions = new()
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
            RefreshToken? token = await readSingleService.Get(x => x.Token == refreshToken, x => x.Include(x => x.User));
            if (token is null || token.JwtId != jwtId || token.UserId != userId)
                throw new Exception("Invalid token");

            (string newJwt, Guid newJwtId) = CreateJWTAndId(token.User);
            token.JwtId = newJwtId;
            await updateService.Update(token);

            return newJwt;
        }

        public Task InvalidateAllTokensForUser(Guid userId) => deleteRangeService.Delete(x => x.UserId == userId);

        public Task InvalidateRefreshToken(Guid refreshToken) => deleteService.Delete(x => x.Token == refreshToken);
    }
}
