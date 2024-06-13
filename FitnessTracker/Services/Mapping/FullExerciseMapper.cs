using FitnessTracker.DTOs;
using FitnessTracker.Models;
using FitnessTracker.Services.Mapping;

namespace FitnessTracker.Services.Mapping
{
    public class FullExerciseMapper(IEntityMapper<Equipment, EquipmentDTO> equipmentMapper, IEntityMapper<MuscleGroup, MuscleGroupDTO> muscleGroupMapper, IEntityMapper<Muscle, MuscleDTO> muscleMapper) : IEntityMapper<Exercise, object>
    {
        public object Map(Exercise entity)
        {
            return new
            {
                entity.Id,
                entity.Name,
                entity.Description,
                Image = entity.EncodedImage,
                PrimaryMuscleGroups = entity.PrimaryMuscleGroups.Select(muscleGroupMapper.Map),
                SecondaryMuscleGroups = entity.SecondaryMuscleGroups.Select(muscleGroupMapper.Map),
                PrimaryMuscles = entity.PrimaryMuscles.Select(muscleMapper.Map),
                SecondaryMuscles = entity.SecondaryMuscles.Select(muscleMapper.Map),
                Equipment = entity.Equipment.Select(equipmentMapper.Map),
            };
        }

        public Exercise Map(object dto) => new();
    }
}
