using FitnessTracker.DTOs.Responses.Equipment;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class EquipmentController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleEquipmentResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get() => Ok(await equipmentService.GetAll());
    }
}
