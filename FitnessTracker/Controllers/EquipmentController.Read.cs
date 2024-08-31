using FitnessTracker.DTOs.Responses.MuscleGroup;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class EquipmentController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleMuscleGroupResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            IEnumerable<Models.Equipment> muscleGroups = await readRangeService.Get(null);
            return Ok(muscleGroups.Select(responseMapper.Map));
        }
    }
}
