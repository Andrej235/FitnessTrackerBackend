using FitnessTracker.Services.ModelServices.MuscleService;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Route("api/muscle")]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public partial class MuscleController(IMuscleService muscleService) : ControllerBase
    {
        private readonly IMuscleService muscleService = muscleService;
    }
}
