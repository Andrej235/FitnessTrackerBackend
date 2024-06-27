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
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update([FromBody] UpdateExerciseRequestDTO request)
        {
            var exercise = await readSingleService.Get(x => x.Id == request.Id, "none");
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

            return NoContent();
        }
    }
}
