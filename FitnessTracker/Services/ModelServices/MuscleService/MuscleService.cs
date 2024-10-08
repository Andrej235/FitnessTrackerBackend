﻿using FitnessTracker.DTOs.Requests.Muscle;
using FitnessTracker.DTOs.Responses.Muscle;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.ModelServices.MuscleService
{
    public partial class MuscleService(ICreateService<Muscle> createService,
                                       ICreateRangeService<Muscle> createRangeService,
                                       IReadRangeService<Muscle> readRangeService,
                                       IDeleteService<Muscle> deleteService,
                                       IRequestMapper<CreateMuscleRequestDTO, Muscle> requestMapper,
                                       IResponseMapper<Muscle, SimpleMuscleResponseDTO> responseMapper) : IMuscleService
    {
        private readonly ICreateService<Muscle> createService = createService;
        private readonly ICreateRangeService<Muscle> createRangeService = createRangeService;
        private readonly IReadRangeService<Muscle> readRangeService = readRangeService;
        private readonly IDeleteService<Muscle> deleteService = deleteService;
        private readonly IRequestMapper<CreateMuscleRequestDTO, Muscle> requestMapper = requestMapper;
        private readonly IResponseMapper<Muscle, SimpleMuscleResponseDTO> responseMapper = responseMapper;
    }
}
