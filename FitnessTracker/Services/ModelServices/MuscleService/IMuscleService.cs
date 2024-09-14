using FitnessTracker.DTOs.Requests.Muscle;
using FitnessTracker.DTOs.Responses.Muscle;

namespace FitnessTracker.Services.ModelServices.MuscleService
{
    public interface IMuscleService
    {
        Task<IEnumerable<SimpleMuscleResponseDTO>> GetAll();
        Task Delete(int id);
        Task Create(CreateMuscleRequestDTO request);
        Task Create(IEnumerable<CreateMuscleRequestDTO> request);
    }
}
