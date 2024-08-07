﻿using FitnessTracker.DTOs.Requests.Exercise;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Request.ExerciseMappers
{
    public class CreateExerciseRequestMapper : IRequestMapper<CreateExerciseRequestDTO, Exercise>
    {
        public Exercise Map(CreateExerciseRequestDTO from) => new()
        {
            Name = from.Name,
            Description = from.Description,
            Image = from.Image ?? "",
        };
    }
}
