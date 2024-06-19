using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.DTOs;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;
using System.Collections.Generic;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.DTOs.Requests.Exercise;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.DTOs.Responses.Exercises;

namespace FitnessTracker.Controllers
{
    [Route("api/exercise")]
    [ApiController]
    public class ExerciseController(ICreateService<Exercise> createService,
                                    IReadService<Exercise> readService,
                                    ICreateRangeService<EquipmentUsage> equipmetUsageCreateRangeService,
                                    ICreateRangeService<PrimaryMuscleGroupInExercise> primaryMuscleGroupCreateRangeService,
                                    ICreateRangeService<PrimaryMuscleInExercise> primaryMuscleCreateRangeService,
                                    ICreateRangeService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupCreateRangeService,
                                    ICreateRangeService<SecondaryMuscleInExercise> secondaryMuscleCreateRangeService,
                                    IRequestMapper<CreateExerciseRequestDTO, Exercise> createRequestMapper,
                                    IResponseMapper<Exercise, DetailedExerciseResponseDTO> detailedResponseMapper) : ControllerBase
    {
        private readonly ICreateService<Exercise> createService = createService;
        private readonly ICreateRangeService<EquipmentUsage> equipmetUsageCreateRangeService = equipmetUsageCreateRangeService;
        private readonly ICreateRangeService<PrimaryMuscleGroupInExercise> primaryMuscleGroupCreateRangeService = primaryMuscleGroupCreateRangeService;
        private readonly ICreateRangeService<PrimaryMuscleInExercise> primaryMuscleCreateRangeService = primaryMuscleCreateRangeService;
        private readonly ICreateRangeService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupCreateRangeService = secondaryMuscleGroupCreateRangeService;
        private readonly ICreateRangeService<SecondaryMuscleInExercise> secondaryMuscleCreateRangeService = secondaryMuscleCreateRangeService;
        private readonly IRequestMapper<CreateExerciseRequestDTO, Exercise> createRequestMapper = createRequestMapper;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? q, [FromQuery] int? limit, [FromQuery] int? offset, [FromQuery] string? include)
        {
            var exercises = await readService.Get(q, offset, limit, include);
            return Ok(exercises.Select(detailedResponseMapper.Map));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExerciseRequestDTO request)
        {
            var newExerciseIdObj = await createService.Add(createRequestMapper.Map(request));
            if (newExerciseIdObj is not int newExerciseId)
                return BadRequest();

            await equipmetUsageCreateRangeService.Add(request.Equipment.Select(x => new EquipmentUsage
            {
                ExerciseId = newExerciseId,
                EquipmentId = x
            }));
            await primaryMuscleGroupCreateRangeService.Add(request.PrimaryMuscleGroups.Select(x => new PrimaryMuscleGroupInExercise
            {
                ExerciseId = newExerciseId,
                MuscleGroupId = x
            }));
            await primaryMuscleCreateRangeService.Add(request.PrimaryMuscles.Select(x => new PrimaryMuscleInExercise
            {
                ExerciseId = newExerciseId,
                MuscleId = x
            }));
            await secondaryMuscleGroupCreateRangeService.Add(request.SecondaryMuscleGroups.Select(x => new SecondaryMuscleGroupInExercise
            {
                ExerciseId = newExerciseId,
                MuscleGroupId = x
            }));
            await secondaryMuscleCreateRangeService.Add(request.SecondaryMuscles.Select(x => new SecondaryMuscleInExercise
            {
                ExerciseId = newExerciseId,
                MuscleId = x
            }));
            return Ok();
        }
    }
}