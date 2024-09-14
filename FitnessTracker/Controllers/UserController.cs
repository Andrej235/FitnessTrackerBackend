using FitnessTracker.Services.ModelServices.UserService;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [Route("api/user")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public partial class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService userService = userService;
    }
}
