using FitnessTracker.DTOs;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping
{
    public class UserMapper : IEntityMapper<User, UserDTO>
    {
        public UserDTO Map(User entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
        };

        public User Map(UserDTO dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Email = dto.Email,
        };
    }
}
