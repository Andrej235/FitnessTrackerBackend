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
