using FitnessTracker.Services.ModelServices.SplitService;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Route("api/split")]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public partial class SplitController(ISplitService splitService) : ControllerBase
    {
        private readonly ISplitService splitService = splitService;
    }
}
