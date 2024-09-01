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
            try
            {
                Exercise newExercise = await createService.Add(createRequestMapper.Map(request));

                await equipmetUsageCreateRangeService.Add(request.Equipment.Select(x => new EquipmentUsage
                {
                    ExerciseId = newExercise.Id,
                    EquipmentId = x
                }));

                await primaryMuscleGroupCreateRangeService.Add(request.PrimaryMuscleGroups.Select(x => new PrimaryMuscleGroupInExercise
                {
                    ExerciseId = newExercise.Id,
                    MuscleGroupId = x
                }));

                await primaryMuscleCreateRangeService.Add(request.PrimaryMuscles.Select(x => new PrimaryMuscleInExercise
                {
                    ExerciseId = newExercise.Id,
                    MuscleId = x
                }));

                await secondaryMuscleGroupCreateRangeService.Add(request.SecondaryMuscleGroups.Select(x => new SecondaryMuscleGroupInExercise
                {
                    ExerciseId = newExercise.Id,
                    MuscleGroupId = x
                }));

                await secondaryMuscleCreateRangeService.Add(request.SecondaryMuscles.Select(x => new SecondaryMuscleInExercise
                {
                    ExerciseId = newExercise.Id,
                    MuscleId = x
                }));

                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetErrorMessage());
            }
        }
    }
}
