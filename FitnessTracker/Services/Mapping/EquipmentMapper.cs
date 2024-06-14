using FitnessTracker.DTOs;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping
{
    public class EquipmentMapper : IEntityMapper<Equipment, EquipmentDTO>
    {
        public EquipmentDTO Map(Equipment entity) => new()
        {
            Id = entity.Id,
            Name = entity.Name,
        };

        public Equipment Map(EquipmentDTO dto) => new()
        {
            Id = dto.Id,
            Name = dto.Name,
        };
    }
}
