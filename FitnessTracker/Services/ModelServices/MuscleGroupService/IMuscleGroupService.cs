using FitnessTracker.DTOs.Requests.MuscleGroup;
using FitnessTracker.DTOs.Responses.MuscleGroup;

namespace FitnessTracker.Services.ModelServices.MuscleGroupService
{
    public interface IMuscleGroupService
    {
        Task Create(CreateMuscleGroupRequestDTO request);
        Task Create(IEnumerable<CreateMuscleGroupRequestDTO> request);
        Task<IEnumerable<SimpleMuscleGroupResponseDTO>> GetAll();
        Task<IEnumerable<DetailedMuscleGroupResponseDTO>> GetAllDetailed();
        Task Delete(int id);
    }
}
