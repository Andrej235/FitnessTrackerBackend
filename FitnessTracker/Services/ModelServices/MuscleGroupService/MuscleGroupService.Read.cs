using FitnessTracker.DTOs.Responses.MuscleGroup;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.ModelServices.MuscleGroupService
{
    public partial class MuscleGroupService
    {
        public async Task<IEnumerable<SimpleMuscleGroupResponseDTO>> GetAll()
        {
            IEnumerable<Models.MuscleGroup> muscleGroups = await readRangeService.Get(null);
            return muscleGroups.Select(responseMapper.Map);
        }

        public async Task<IEnumerable<DetailedMuscleGroupResponseDTO>> GetAllDetailed()
        {
            IEnumerable<Models.MuscleGroup> muscleGroups = await readRangeService.Get(null, queryBuilder: x => x.Include(x => x.Muscles));
            return muscleGroups.Select(detailedResponseMapper.Map);
        }
    }
}
