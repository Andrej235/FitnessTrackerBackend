using ProjectGym.DTOs;
using ProjectGym;
using ProjectGym.Models;
using ProjectGym.Services.Mapping;

namespace FitnessTracker.Services.Mapping
{
    public class FullExerciseMapper : IEntityMapper<Exercise, object>
    {
        public object Map(Exercise entity)
        {
            var muscleGroupMapper = entity.PrimaryMuscleGroups.Any() || entity.SecondaryMuscleGroups.Any() ? Program.GetService(typeof(IEntityMapper<MuscleGroup, MuscleGroupDTO>)) as IEntityMapper<MuscleGroup, MuscleGroupDTO> : null;
            var muscleMapper = entity.PrimaryMuscles.Any() || entity.SecondaryMuscles.Any() ? Program.GetService(typeof(IEntityMapper<Muscle, MuscleDTO>)) as IEntityMapper<Muscle, MuscleDTO> : null;

            return new
            {
                entity.Id,
                entity.Name,
                PrimaryMuscleGroups = muscleGroupMapper is null ? [] : entity.PrimaryMuscleGroups.Select(muscleGroupMapper.Map),
                SecondaryMuscleGroups = muscleGroupMapper is null ? [] : entity.SecondaryMuscleGroups.Select(muscleGroupMapper.Map),
                PrimaryMuscles = muscleMapper is null ? [] : entity.PrimaryMuscles.Select(muscleMapper.Map),
                SecondaryMuscles = muscleMapper is null ? [] : entity.SecondaryMuscles.Select(muscleMapper.Map),
                Equipment = !entity.Equipment.Any() || Program.GetService(typeof(IEntityMapper<Equipment, EquipmentDTO>)) is not IEntityMapper<Equipment, EquipmentDTO> equipmentMapper ? [] : entity.Equipment.Select(equipmentMapper.Map),
                Aliases = !entity.Aliases.Any() || Program.GetService(typeof(IEntityMapper<Alias, AliasDTO>)) is not IEntityMapper<Alias, AliasDTO> aliasMapper ? [] : entity.Aliases.Select(aliasMapper.Map),
                //Bookmarks = !entity.Bookmarks.Any() || Program.serviceProvider.GetService(typeof(IEntityMapper<User, UserDTO>)) is not IEntityMapper<User, UserDTO> bookmarkMapper ? [] : entity.Bookmarks.Select(bookmarkMapper.Map)
            };
        }

        public Exercise Map(object dto) => new();
    }
}
