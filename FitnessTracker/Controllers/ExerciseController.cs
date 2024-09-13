using FitnessTracker.Services.ModelServices.ExerciseService;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [Route("api/exercise")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public partial class ExerciseController(IExerciseService exerciseService) : ControllerBase
    {
        private readonly IExerciseService exerciseService = exerciseService;
    }
}