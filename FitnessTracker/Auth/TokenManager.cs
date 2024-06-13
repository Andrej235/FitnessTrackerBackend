using FitnessTracker.Models;
using Microsoft.IdentityModel.Tokens;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitnessTracker.Auth
{
    public class TokenManager(ConfigurationManager configuration, IReadService<RefreshToken> readService, ICreateService<RefreshToken> createService, IUpdateService<RefreshToken> updateService, IDeleteService<RefreshToken> deleteService)
    {
        private readonly ConfigurationManager configuration = configuration;
        private readonly IReadService<RefreshToken> readService = readService;
        private readonly ICreateService<RefreshToken> createService = createService;
        private readonly IUpdateService<RefreshToken> updateService = updateService;
        private readonly IDeleteService<RefreshToken> deleteService = deleteService;

        public string CreateJWT(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
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

            return tokenString;
        }

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

        public (string jwt, string refresh) CreateJWTAndRefreshToken(User user)
        {
            var (jwt, jwtId) = CreateJWTAndId(user);

            RefreshToken refresh = new()
            {
                JwtId = jwtId,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
            };

            createService.Add(refresh);
            return (jwt, refresh.Token.ToString());
        }

        public async Task<string> RefreshJWT(Guid jwtId, Guid refreshToken, Guid userId)
        {
            var token = await readService.Get(refreshToken, "none");
            if (token is null || token.JwtId != jwtId || token.UserId != userId)
                throw new Exception("Invalid token");

            var (newJwt, newJwtId) = CreateJWTAndId(token.User);
            token.JwtId = newJwtId;
            await updateService.Update(token);

            return newJwt;
        }

        public Task InvalidateAllTokensForUser(Guid userId) => deleteService.DeleteAll(x => x.UserId == userId);

        public Task InvalidateRefreshToken(Guid refreshToken) => deleteService.DeleteFirst(x => x.Token == refreshToken);
    }
}
