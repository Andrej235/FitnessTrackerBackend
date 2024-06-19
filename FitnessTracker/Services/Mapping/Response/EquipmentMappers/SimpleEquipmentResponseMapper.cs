using FitnessTracker.DTOs.Responses.Equipment;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.EquipmentMappers
{
    public class SimpleEquipmentResponseMapper : IResponseMapper<Equipment, SimpleEquipmentResponseDTO>
    {
        public SimpleEquipmentResponseDTO Map(Equipment from)
        {
            return new()
            {
                Id = from.Id,
                Name = from.Name
            };
        }
    }
}
