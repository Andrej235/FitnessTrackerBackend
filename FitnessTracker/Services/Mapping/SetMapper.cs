using FitnessTracker.DTOs;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping
{
    public class SetMapper : IEntityMapper<Set, SetDTO>
    {
        public Set Map(SetDTO dto) => new()
        {
            Id = dto.Id,
            TopRepRange  = dto.RepRange_Top,
            BottomRepRange = dto.RepRange_Bottom,
            ExerciseId = dto.ExerciseId,
        };

        public SetDTO Map(Set entity) => new()
        {
            Id = entity.Id,
            RepRange_Top = entity.TopRepRange ,
            RepRange_Bottom = entity.BottomRepRange,
            ExerciseId = entity.ExerciseId,
        };
    }
}
