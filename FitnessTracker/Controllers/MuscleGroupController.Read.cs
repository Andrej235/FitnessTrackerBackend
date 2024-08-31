using FitnessTracker.DTOs.Responses.MuscleGroup;
using FitnessTracker.Services.Read.Full;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class MuscleGroupController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleMuscleGroupResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            IEnumerable<Models.MuscleGroup> muscleGroups = await readRangeService.Get(x => true);
            return Ok(muscleGroups.Select(responseMapper.Map));
        }

        [HttpGet("detailed")]
        [ProducesResponseType(typeof(IEnumerable<DetailedMuscleGroupResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailed()
        {
            IEnumerable<Models.MuscleGroup> muscleGroups = await readRangeService.Get(x => true, include: x => x.Include(x => x.Muscles));
            return Ok(muscleGroups.Select(detailedResponseMapper.Map));
        }
    }
}
