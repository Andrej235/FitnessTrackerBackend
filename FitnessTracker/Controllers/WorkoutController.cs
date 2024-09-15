using FitnessTracker.Services.ModelServices.WorkoutService;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Route("api/workout")]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public partial class WorkoutController(IWorkoutService workoutService) : ControllerBase
    {
        private readonly IWorkoutService workoutService = workoutService;
    }
}
