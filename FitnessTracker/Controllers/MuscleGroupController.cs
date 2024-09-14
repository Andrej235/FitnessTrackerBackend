using FitnessTracker.Services.ModelServices.MuscleGroupService;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Route("api/musclegroup")]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public partial class MuscleGroupController(IMuscleGroupService muscleGroupService) : ControllerBase
    {
        private readonly IMuscleGroupService muscleGroupService = muscleGroupService;
    }
}
