using FitnessTracker.DTOs.Requests.Exercise;
using FitnessTracker.Models;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class ExerciseController
    {
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateExerciseRequestDTO request)
        {
            object? newExerciseIdObj = await createService.Add(createRequestMapper.Map(request));
            if (newExerciseIdObj is not int newExerciseId)
                return BadRequest();

            _ = await equipmetUsageCreateRangeService.Add(request.Equipment.Select(x => new EquipmentUsage
            {
                ExerciseId = newExerciseId,
                EquipmentId = x
            }));
            _ = await primaryMuscleGroupCreateRangeService.Add(request.PrimaryMuscleGroups.Select(x => new PrimaryMuscleGroupInExercise
            {
                ExerciseId = newExerciseId,
                MuscleGroupId = x
            }));
            _ = await primaryMuscleCreateRangeService.Add(request.PrimaryMuscles.Select(x => new PrimaryMuscleInExercise
            {
                ExerciseId = newExerciseId,
                MuscleId = x
            }));
            _ = await secondaryMuscleGroupCreateRangeService.Add(request.SecondaryMuscleGroups.Select(x => new SecondaryMuscleGroupInExercise
            {
                ExerciseId = newExerciseId,
                MuscleGroupId = x
            }));
            _ = await secondaryMuscleCreateRangeService.Add(request.SecondaryMuscles.Select(x => new SecondaryMuscleInExercise
            {
                ExerciseId = newExerciseId,
                MuscleId = x
            }));
            return Created();
        }
    }
}
