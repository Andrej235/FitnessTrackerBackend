﻿using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.SplitMappers
{
    public class SimpleSplitResponseMapper(IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper) : IResponseMapper<Split, SimpleSplitResponseDTO>
    {
        private readonly IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper = userResponseMapper;

        public SimpleSplitResponseDTO Map(Split from) => new()
        {
            Id = from.Id,
            Name = from.Name,
            Description = from.Description,
            Creator = userResponseMapper.Map(from.Creator),
        };
    }
}
