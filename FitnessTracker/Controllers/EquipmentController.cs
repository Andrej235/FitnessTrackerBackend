using FitnessTracker.Services.ModelServices.EquipmentService;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Route("api/equipment")]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public partial class EquipmentController(IEquipmentService equipmentService) : ControllerBase
    {
        private readonly IEquipmentService equipmentService = equipmentService;
    }
}
