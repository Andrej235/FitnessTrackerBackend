using FitnessTracker.DTOs;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping
{
    public class WorkoutMapper : IEntityMapper<Workout, WorkoutDTO>
    {
        public Workout Map(WorkoutDTO dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
            CreatorId = dto.CreatorId,
            IsPublic = dto.IsPublic,
        };

        public WorkoutDTO Map(Workout entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            IsPublic = entity.IsPublic,
            CreatorId = entity.CreatorId,
            WorkoutSetIds = entity.Sets.Select(x => x.Id),
        };
    }
}
