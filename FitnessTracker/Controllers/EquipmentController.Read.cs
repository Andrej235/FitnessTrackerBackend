using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class EquipmentController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? limit, [FromQuery] int? offset, [FromQuery] string? include)
        {
            var muscleGroups = await readRangeService.Get(x => true, offset, limit, include);
            return Ok(muscleGroups.Select(responseMapper.Map));
        }
    }
}
