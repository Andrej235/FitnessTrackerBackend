using FitnessTracker.DTOs.Requests.Equipment;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class EquipmentController
    {
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEquipmentRequestDTO request)
        {
            await createService.Add(requestMapper.Map(request));
            return Ok();
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("range")]
        public async Task<IActionResult> Create([FromBody] IEnumerable<CreateEquipmentRequestDTO> request)
        {
            await createRangeService.Add(request.Select(requestMapper.Map));
            return Ok();
        }
    }
}
