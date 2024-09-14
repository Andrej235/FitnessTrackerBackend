using FitnessTracker.DTOs.Responses.MuscleGroup;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class MuscleGroupController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleMuscleGroupResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get() => Ok(await muscleGroupService.GetAll());

        [HttpGet("detailed")]
        [ProducesResponseType(typeof(IEnumerable<DetailedMuscleGroupResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailed() => Ok(await muscleGroupService.GetAllDetailed());
    }
}
