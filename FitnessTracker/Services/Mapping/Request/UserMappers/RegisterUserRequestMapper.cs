﻿using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.Models;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.Mapping.Request.UserMappers
{
    public class RegisterUserRequestMapper : IRequestMapper<RegisterUserRequestDTO, User>
    {
        public User Map(RegisterUserRequestDTO from)
        {
            byte[] salt = HashingService.GenerateSalt();
            byte[] hash = from.Password.ToHash(salt);

            return new User
            {
                Name = from.Username,
                Username = from.Username,
                Email = from.Email,
                Bio = "",
                Salt = salt,
                PasswordHash = hash,
                Role = Role.Unverified,
                JoinedAt = DateTime.UtcNow,
            };
        }
    }
}
