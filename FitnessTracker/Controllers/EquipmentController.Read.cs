using FitnessTracker.DTOs.Responses.MuscleGroup;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class EquipmentController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleMuscleGroupResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] int? limit, [FromQuery] int? offset, [FromQuery] string? include)
        {
            IEnumerable<Models.Equipment> muscleGroups = await readRangeService.Get(x => true, offset, limit, include);
            return Ok(muscleGroups.Select(responseMapper.Map));
        }
    }
}
