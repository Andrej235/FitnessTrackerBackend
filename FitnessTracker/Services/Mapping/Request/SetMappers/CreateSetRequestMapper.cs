﻿using FitnessTracker.DTOs.Requests.Set;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Request.SetMappers
{
    public class CreateSetRequestMapper : IRequestMapper<CreateSetRequestDTO, Set>
    {
        public Set Map(CreateSetRequestDTO from) => new()
        {
            ExerciseId = from.ExerciseId,
            BottomRepRange = from.BottomRepRange,
            TopRepRange = from.TopRepRange,
            RiR = from.RiR,
            Type = from.Type,
        };
    }
}
