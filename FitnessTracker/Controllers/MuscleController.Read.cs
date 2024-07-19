using FitnessTracker.DTOs.Responses.Muscle;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class MuscleController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleMuscleResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] int? limit, [FromQuery] int? offset, [FromQuery] string? include)
        {
            IEnumerable<Models.Muscle> muscleGroups = await readRangeService.Get(x => true, offset, limit, include);
            return Ok(muscleGroups.Select(responseMapper.Map));
        }
    }
}
