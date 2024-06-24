using FitnessTracker.DTOs.Requests.Muscle;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class MuscleController
    {
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMuscleRequestDTO request)
        {
            await createService.Add(requestMapper.Map(request));
            return Ok();
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("range")]
        public async Task<IActionResult> Create([FromBody] IEnumerable<CreateMuscleRequestDTO> request)
        {
            await createRangeService.Add(request.Select(requestMapper.Map));
            return Ok();
        }
    }
}
