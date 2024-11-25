using FitnessTracker.DTOs.Responses.User;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [HttpGet("{username}/settings")]
        [ProducesResponseType(typeof(UserSettingsResponseDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSettings(string username) => Ok(await userService.GetSettings(username));
    }
}
