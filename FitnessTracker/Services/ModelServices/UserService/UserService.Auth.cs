using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.DTOs.Responses.AuthTokens;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        private readonly HashSet<string> restrictedUsernames =
        [
            "admin",
            "exercises",
            "exercise",
            "workout",
            "workouts",
            "split",
            "splits",
            "authentication",
            "auth",
            "register",
            "login",
            "resetpassword",
            "user"
        ];

        public async Task<SimpleJWTResponseDTO> Register(RegisterUserRequestDTO request, IResponseCookies cookies)
        {
            ValidateUsername(request.Username);

            if (!ValidEmailRegex().IsMatch(request.Email.Trim()) || request.Password.Length < 8)
                throw new InvalidRequestDTOException("Invalid registration details");

            User mapped = registrationMapper.Map(request);
            User newUser = await createService.Add(mapped);

            _ = await settingsCreateService.Add(new Models.UserSettings
            {
                UserId = newUser.Id,
                PublicStreak = true,
                PublicCompletedWorkouts = true,
                PublicCurrentSplit = true,
                PublicFollowing = true,
                PublicLikedWorkouts = true,
                PublicLikedSplits = true,
                PublicFavoriteWorkouts = true,
                PublicFavoriteSplits = true,
            });

            string jwt = await tokenManager.GenerateJWTAndRefreshToken(newUser, cookies);
            await emailConfirmationSender.SendEmailConfirmation(newUser.Email, newUser.Id);
            return jwtResponseMapper.Map(jwt);
        }

        private void ValidateUsername(string username)
        {
            if (!ValidUsernameRegex().IsMatch(username))
                throw new InvalidRequestDTOException("Username is invalid");

            if (restrictedUsernames.Contains(username.ToLower()))
                throw new InvalidRequestDTOException("Username is reserved");
        }

        public async Task<SimpleJWTResponseDTO> Login(LoginUserRequestDTO request, IResponseCookies cookies)
        {
            if (!ValidEmailRegex().IsMatch(request.Email) || request.Password.Length < 8)
                throw new InvalidRequestDTOException("Incorrect email or password");

            User user = await readSingleService.Get(x => x.Email == request.Email) ?? throw new InvalidLoginCredentialsException("Incorrect email or password");

            byte[] hash = request.Password.ToHash(user.Salt);
            if (!user.PasswordHash.SequenceEqual(hash))
                throw new InvalidLoginCredentialsException("Incorrect email or password");

            string jwt = await tokenManager.GenerateJWTAndRefreshToken(user, cookies);
            return jwtResponseMapper.Map(jwt);
        }

        public async Task<SimpleJWTResponseDTO> Refresh(Guid jwtId, Guid userId, Guid refreshToken)
        {
            if (jwtId == default || userId == default || refreshToken == default)
                throw new UnauthorizedException();

            string newJwt = await tokenManager.RefreshJWT(jwtId, refreshToken, userId);
            return jwtResponseMapper.Map(newJwt);
        }

        public Task Logout(Guid refreshToken) => tokenManager.InvalidateRefreshToken(refreshToken);
    }
}
