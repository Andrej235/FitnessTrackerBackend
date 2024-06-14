using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.Models;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.Mapping.Request
{
    public class RegisterUserRequestMapper : IRequestMapper<RegisterUserRequestDTO, User>
    {
        public User Map(RegisterUserRequestDTO from)
        {
            var salt = HashingService.GenerateSalt();
            var hash = from.Password.ToHash(salt);

            return new User
            {
                Name = from.Name,
                Email = from.Email,
                Salt = salt,
                PasswordHash = hash,
                Role = Role.User
            };
        }
    }
}
