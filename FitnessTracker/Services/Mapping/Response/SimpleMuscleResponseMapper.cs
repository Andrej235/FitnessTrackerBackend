﻿using FitnessTracker.DTOs.Responses.Muscle;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response
{
    public class SimpleMuscleResponseMapper : IResponseMapper<Muscle, SimpleMuscleResponseDTO>
    {
        public SimpleMuscleResponseDTO Map(Muscle from)
        {
            return new()
            {
                Id = from.Id,
                Name = from.Name,
                MuscleGroupId = from.MuscleGroupId,
            };
        }
    }
}
