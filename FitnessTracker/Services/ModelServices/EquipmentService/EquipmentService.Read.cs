using FitnessTracker.DTOs.Responses.Equipment;
using FitnessTracker.Models;

namespace FitnessTracker.Services.ModelServices.EquipmentService
{
    public partial class EquipmentService
    {
        public async Task<IEnumerable<SimpleEquipmentResponseDTO>> GetAll()
        {
            IEnumerable<Equipment> equipment = await readRangeService.Get(null);
            return equipment.Select(responseMapper.Map);
        }
    }
}
