using FitnessTracker.DTOs.Requests.Muscle;
using FitnessTracker.Exceptions;

namespace FitnessTracker.Services.ModelServices.MuscleService
{
    public partial class MuscleService
    {
        public Task Create(CreateMuscleRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new InvalidRequestDTOException(nameof(request.Name) + " cannot be null or empty");

            return createService.Add(requestMapper.Map(request));
        }

        public Task Create(IEnumerable<CreateMuscleRequestDTO> request)
        {
            IEnumerable<CreateMuscleRequestDTO> validRequests = request.Where(x => !string.IsNullOrWhiteSpace(x.Name));
            return createRangeService.Add(validRequests.Select(requestMapper.Map));
        }
    }
}
