using FitnessTracker.DTOs.Responses.Pins;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [HttpGet("{username}/pins")]
        [ProducesResponseType(typeof(IEnumerable<PinResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPinsFor(string username) => Ok(await userService.GetPinsFor(username));
    }
}
