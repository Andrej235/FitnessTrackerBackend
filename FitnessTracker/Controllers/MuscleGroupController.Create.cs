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
        public async Task<IActionResult> Create([FromBody] CreateMuscleGroupRequestDTO request)
        {
            await createService.Add(requestMapper.Map(request));
            return Created();
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("range")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] IEnumerable<CreateMuscleGroupRequestDTO> request)
        {
            await createRangeService.Add(request.Select(requestMapper.Map));
            return Created();
        }
    }
}
