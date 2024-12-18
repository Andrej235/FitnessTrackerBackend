﻿using FitnessTracker.Exceptions;
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
                              IReadSingleSelectedService<RefreshToken> readSingleSelectedService,
                              ICreateService<RefreshToken> createService,
                              IExecuteUpdateService<RefreshToken> executeUpdateService,
                              IDeleteService<RefreshToken> deleteService) : ITokenManager
    {
        private readonly ConfigurationManager configuration = configuration;
        private readonly IReadSingleSelectedService<RefreshToken> readSingleSelectedService = readSingleSelectedService;
        private readonly ICreateService<RefreshToken> createService = createService;
        private readonly IExecuteUpdateService<RefreshToken> executeUpdateService = executeUpdateService;
        private readonly IDeleteService<RefreshToken> deleteService = deleteService;

        private (string jwt, Guid jwtId) CreateJWTAndId(Guid userId, string role)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
            Guid jwtId = Guid.NewGuid();

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, jwtId.ToString()),
                        new Claim(ClaimTypes.Role, role)
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

        public async Task<string> GenerateJWTAndRefreshToken(Guid userId, string role, IResponseCookies cookies)
        {
            (string jwt, Guid jwtId) = CreateJWTAndId(userId, role);

            RefreshToken refresh = new()
            {
                JwtId = jwtId,
                UserId = userId,
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
            var token = await readSingleSelectedService.Get(
                x => new
                {
                    x.JwtId,
                    x.UserId,
                    x.User.Role
                },
                x => x.Token == refreshToken);

            if (token is null || token.JwtId != jwtId || token.UserId != userId)
                throw new InvalidRequestDTOException("Invalid token");

            (string newJwt, Guid newJwtId) = CreateJWTAndId(token.UserId, token.Role);
            await executeUpdateService.Update(x => x.Token == refreshToken, x => x.SetProperty(x => x.JwtId, newJwtId));

            return newJwt;
        }

        public Task InvalidateAllTokensForUser(Guid userId) => deleteService.Delete(x => x.UserId == userId);

        public Task InvalidateRefreshToken(Guid refreshToken) => deleteService.Delete(x => x.Token == refreshToken);
    }
}
