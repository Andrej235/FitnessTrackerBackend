﻿using FitnessTracker.DTOs.Responses.Split;

namespace FitnessTracker.DTOs.Responses.User
{
    public class DetailedUserResponseDTO : BaseUserResponseDTO
    {
        public int Followers { get; set; }
        public int Following { get; set; }
        public int CompletedWorkouts { get; set; }
        public DetailedUserSplitResponseDTO? CurrentSplit { get; set; }
    }
}
