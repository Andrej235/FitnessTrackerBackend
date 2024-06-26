﻿using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.WorkoutMappers
{
    public class SimpleWorkoutCommentResponseMapper(IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper) : IResponseMapper<WorkoutComment, SimpleWorkoutCommentResponseDTO>
    {
        private readonly IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper = userResponseMapper;

        public SimpleWorkoutCommentResponseDTO Map(WorkoutComment from)
        {
            return new()
            {
                Id = from.Id,
                Text = from.Text,
                CreatedAt = from.CreatedAt,
                Creator = userResponseMapper.Map(from.Creator),
                LikeCount = from.Likes.Count,
            };
        }
    }
}
