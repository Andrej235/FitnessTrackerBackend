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
                                    IUpdateService<Exercise> updateService,
                                    IDeleteService<Exercise> deleteService,
                                    ICreateRangeService<EquipmentUsage> equipmetUsageCreateRangeService,
                                    ICreateRangeService<PrimaryMuscleGroupInExercise> primaryMuscleGroupCreateRangeService,
                                    ICreateRangeService<PrimaryMuscleInExercise> primaryMuscleCreateRangeService,
                                    ICreateRangeService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupCreateRangeService,
                                    ICreateRangeService<SecondaryMuscleInExercise> secondaryMuscleCreateRangeService,
                                    IDeleteRangeService<EquipmentUsage> equipmetUsageDeleteRangeService,
                                    IDeleteRangeService<PrimaryMuscleGroupInExercise> primaryMuscleGroupDeleteRangeService,
                                    IDeleteRangeService<PrimaryMuscleInExercise> primaryMuscleDeleteRangeService,
                                    IDeleteRangeService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupDeleteRangeService,
                                    IDeleteRangeService<SecondaryMuscleInExercise> secondaryMuscleDeleteRangeService,
                                    IRequestMapper<CreateExerciseRequestDTO, Exercise> createRequestMapper,
                                    IResponseMapper<Exercise, DetailedExerciseResponseDTO> detailedResponseMapper) : ControllerBase
    {
        private readonly ICreateService<Exercise> createService = createService;
        private readonly IReadService<Exercise> readService = readService;
        private readonly IUpdateService<Exercise> updateService = updateService;
        private readonly IDeleteService<Exercise> deleteService = deleteService;
        private readonly ICreateRangeService<EquipmentUsage> equipmetUsageCreateRangeService = equipmetUsageCreateRangeService;
        private readonly ICreateRangeService<PrimaryMuscleGroupInExercise> primaryMuscleGroupCreateRangeService = primaryMuscleGroupCreateRangeService;
        private readonly ICreateRangeService<PrimaryMuscleInExercise> primaryMuscleCreateRangeService = primaryMuscleCreateRangeService;
        private readonly ICreateRangeService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupCreateRangeService = secondaryMuscleGroupCreateRangeService;
        private readonly ICreateRangeService<SecondaryMuscleInExercise> secondaryMuscleCreateRangeService = secondaryMuscleCreateRangeService;
        private readonly IDeleteRangeService<EquipmentUsage> equipmetUsageDeleteRangeService = equipmetUsageDeleteRangeService;
        private readonly IDeleteRangeService<PrimaryMuscleGroupInExercise> primaryMuscleGroupDeleteRangeService = primaryMuscleGroupDeleteRangeService;
        private readonly IDeleteRangeService<PrimaryMuscleInExercise> primaryMuscleDeleteRangeService = primaryMuscleDeleteRangeService;
        private readonly IDeleteRangeService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupDeleteRangeService = secondaryMuscleGroupDeleteRangeService;
        private readonly IDeleteRangeService<SecondaryMuscleInExercise> secondaryMuscleDeleteRangeService = secondaryMuscleDeleteRangeService;
        private readonly IRequestMapper<CreateExerciseRequestDTO, Exercise> createRequestMapper = createRequestMapper;
        private readonly IResponseMapper<Exercise, DetailedExerciseResponseDTO> detailedResponseMapper = detailedResponseMapper;

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

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateExerciseRequestDTO request)
        {
            var exercise = await readService.Get(x => x.Id == request.Id, "none");
            if (exercise is null)
                return NotFound();

            await equipmetUsageDeleteRangeService.Delete(x => x.ExerciseId == request.Id);
            await primaryMuscleGroupDeleteRangeService.Delete(x => x.ExerciseId == request.Id);
            await primaryMuscleDeleteRangeService.Delete(x => x.ExerciseId == request.Id);
            await secondaryMuscleGroupDeleteRangeService.Delete(x => x.ExerciseId == request.Id);
            await secondaryMuscleDeleteRangeService.Delete(x => x.ExerciseId == request.Id);

            await equipmetUsageCreateRangeService.Add(request.Equipment.Select(x => new EquipmentUsage
            {
                ExerciseId = request.Id,
                EquipmentId = x
            }));
            await primaryMuscleGroupCreateRangeService.Add(request.PrimaryMuscleGroups.Select(x => new PrimaryMuscleGroupInExercise
            {
                ExerciseId = request.Id,
                MuscleGroupId = x
            }));
            await primaryMuscleCreateRangeService.Add(request.PrimaryMuscles.Select(x => new PrimaryMuscleInExercise
            {
                ExerciseId = request.Id,
                MuscleId = x
            }));
            await secondaryMuscleGroupCreateRangeService.Add(request.SecondaryMuscleGroups.Select(x => new SecondaryMuscleGroupInExercise
            {
                ExerciseId = request.Id,
                MuscleGroupId = x
            }));
            await secondaryMuscleCreateRangeService.Add(request.SecondaryMuscles.Select(x => new SecondaryMuscleInExercise
            {
                ExerciseId = request.Id,
                MuscleId = x
            }));

            exercise.Name = request.Name;
            exercise.Description = request.Description;
            exercise.Image = request.Image ?? "";
            await updateService.Update(exercise);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await deleteService.Delete(x => x.Id == id);
            return Ok();
        }
    }
}