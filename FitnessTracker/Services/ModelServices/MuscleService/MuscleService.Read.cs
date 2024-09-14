using FitnessTracker.DTOs.Responses.Muscle;

namespace FitnessTracker.Services.ModelServices.MuscleService
{
    public partial class MuscleService
    {
        public async Task<IEnumerable<SimpleMuscleResponseDTO>> GetAll()
        {
            IEnumerable<Models.Muscle> muscleGroups = await readRangeService.Get(null);
            return muscleGroups.Select(responseMapper.Map);
        }
    }
}
