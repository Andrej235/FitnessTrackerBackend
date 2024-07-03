﻿namespace FitnessTracker.DTOs.Responses.User
{
    public class DetailedPublicUserResponseDTO : SimpleUserResponseDTO
    {
        public int Followers { get; set; }
        public int Following { get; set; }
        public int TotalCompletedWorkouts { get; set; }
        public bool IsFollowing { get; set; }
    }
}