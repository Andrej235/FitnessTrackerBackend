using FitnessTracker.DTOs.Responses.Muscle;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class MuscleController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleMuscleResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            IEnumerable<Models.Muscle> muscleGroups = await readRangeService.Get(null);
            return Ok(muscleGroups.Select(responseMapper.Map));
        }
    }
}
