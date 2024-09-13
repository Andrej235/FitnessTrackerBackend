using FitnessTracker.DTOs.Requests.Exercise;
using FitnessTracker.Models;

namespace FitnessTracker.Services.ModelServices.ExerciseService
{
    public partial class ExerciseService
    {
        public async Task Create(CreateExerciseRequestDTO newExerciseRequestDTO)
        {
            Exercise newExercise = await createService.Add(createRequestMapper.Map(newExerciseRequestDTO));

            await equipmetUsageCreateRangeService.Add(newExerciseRequestDTO.Equipment.Select(x => new EquipmentUsage
            {
                ExerciseId = newExercise.Id,
                EquipmentId = x
            }));

            await primaryMuscleGroupCreateRangeService.Add(newExerciseRequestDTO.PrimaryMuscleGroups.Select(x => new PrimaryMuscleGroupInExercise
            {
                ExerciseId = newExercise.Id,
                MuscleGroupId = x
            }));

            await primaryMuscleCreateRangeService.Add(newExerciseRequestDTO.PrimaryMuscles.Select(x => new PrimaryMuscleInExercise
            {
                ExerciseId = newExercise.Id,
                MuscleId = x
            }));

            await secondaryMuscleGroupCreateRangeService.Add(newExerciseRequestDTO.SecondaryMuscleGroups.Select(x => new SecondaryMuscleGroupInExercise
            {
                ExerciseId = newExercise.Id,
                MuscleGroupId = x
            }));

            await secondaryMuscleCreateRangeService.Add(newExerciseRequestDTO.SecondaryMuscles.Select(x => new SecondaryMuscleInExercise
            {
                ExerciseId = newExercise.Id,
                MuscleId = x
            }));
        }
    }
}
