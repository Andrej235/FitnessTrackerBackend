using FitnessTracker.DTOs.Responses.MuscleGroup;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class MuscleGroupController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleMuscleGroupResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] int? limit, [FromQuery] int? offset)
        {
            var muscleGroups = await readRangeService.Get(x => true, offset, limit, "none");
            return Ok(muscleGroups.Select(responseMapper.Map));
        }

        [HttpGet("detailed")]
        [ProducesResponseType(typeof(IEnumerable<DetailedMuscleGroupResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailed([FromQuery] int? limit, [FromQuery] int? offset)
        {
            var muscleGroups = await readRangeService.Get(x => true, offset, limit, "muscles");
            return Ok(muscleGroups.Select(detailedResponseMapper.Map));
        }
    }
}
