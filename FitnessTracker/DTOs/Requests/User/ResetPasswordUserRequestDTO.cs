﻿namespace FitnessTracker.DTOs.Requests.User
{
    public class ResetPasswordUserRequestDTO
    {
        public string Email { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
