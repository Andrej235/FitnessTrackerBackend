using FitnessTracker.DTOs.Requests.MuscleGroup;
using FitnessTracker.Exceptions;

namespace FitnessTracker.Services.ModelServices.MuscleGroupService
{
    public partial class MuscleGroupService
    {
        public Task Create(CreateMuscleGroupRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new InvalidRequestDTOException(nameof(request.Name) + " cannot be null or empty");

            return createService.Add(requestMapper.Map(request));
        }

        public Task Create(IEnumerable<CreateMuscleGroupRequestDTO> request)
        {
            IEnumerable<CreateMuscleGroupRequestDTO> validRequests = request.Where(x => !string.IsNullOrWhiteSpace(x.Name));
            return createRangeService.Add(validRequests.Select(requestMapper.Map));
        }
    }
}
