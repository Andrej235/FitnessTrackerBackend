using FitnessTracker.DTOs.Requests.Equipment;
using FitnessTracker.DTOs.Responses.Equipment;

namespace FitnessTracker.Services.ModelServices.EquipmentService
{
    public interface IEquipmentService
    {
        Task<IEnumerable<SimpleEquipmentResponseDTO>> GetAll();
        Task Delete(int id);
        Task Create(CreateEquipmentRequestDTO request);
        Task Create(IEnumerable<CreateEquipmentRequestDTO> request);
    }
}
