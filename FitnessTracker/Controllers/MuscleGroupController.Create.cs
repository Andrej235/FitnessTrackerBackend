using FitnessTracker.DTOs.Requests.MuscleGroup;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class MuscleGroupController
    {
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateMuscleGroupRequestDTO request)
        {
            await muscleGroupService.Create(request);
            return Created();
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("range")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] IEnumerable<CreateMuscleGroupRequestDTO> request)
        {
            await muscleGroupService.Create(request);
            return Created();
        }
    }
}
