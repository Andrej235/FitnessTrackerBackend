using FitnessTracker.DTOs;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.Mapping
{
    public class MuscleGroupMapper : IEntityMapper<MuscleGroup, MuscleGroupDTO>
    {
        public MuscleGroupDTO Map(MuscleGroup entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            PrimaryInExercises = entity.PrimaryInExercises.Select(x => x.Id),
            SecondaryInExercises = entity.SecondaryInExercises.Select(x => x.Id),
        };

        public MuscleGroup Map(MuscleGroupDTO dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
        };
    }
}
