﻿using FitnessTracker.DTOs.Requests.Set;
using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Request.WorkoutMappers
{
    public class CreateWorkoutRequestMapper(IRequestMapper<CreateSetRequestDTO, Set> setRequestMapper) : IRequestMapper<CreateWorkoutRequestDTO, Workout>
    {
        private readonly IRequestMapper<CreateSetRequestDTO, Set> setRequestMapper = setRequestMapper;

        public Workout Map(CreateWorkoutRequestDTO from) => new()
        {
            Name = from.Name,
            Description = from.Description,
            Sets = from.Sets.Select(setRequestMapper.Map).ToList(),
            CreatedAt = DateTime.Now
        };
    }
}
