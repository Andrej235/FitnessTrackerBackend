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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateEquipmentRequestDTO request)
        {
            try
            {
                _ = await createService.Add(requestMapper.Map(request));
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetErrorMessage());
            }
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("range")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] IEnumerable<CreateEquipmentRequestDTO> request)
        {
            try
            {
                await createRangeService.Add(request.Select(requestMapper.Map));
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetErrorMessage());
            }
        }
    }
}
