using FitnessTracker.DTOs.Requests.Equipment;
using FitnessTracker.Exceptions;

namespace FitnessTracker.Services.ModelServices.EquipmentService
{
    public partial class EquipmentService
    {
        public Task Create(CreateEquipmentRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new InvalidRequestDTOException(nameof(request.Name) + " cannot be null or empty");

            return createService.Add(requestMapper.Map(request));
        }

        public Task Create(IEnumerable<CreateEquipmentRequestDTO> request)
        {
            IEnumerable<CreateEquipmentRequestDTO> validRequests = request.Where(x => !string.IsNullOrWhiteSpace(x.Name));
            return createRangeService.Add(validRequests.Select(requestMapper.Map));
        }
    }
}
