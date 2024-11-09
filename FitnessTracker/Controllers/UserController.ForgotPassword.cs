using FitnessTracker.DTOs.Requests.User;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SendForgotPasswordEmail([FromBody] SendForgotPasswordEmailRequestDTO request)
        {
            await userService.SendForgotPasswordEmail(request);
            return Created();
        }

        [HttpPatch("forgot-password/{code:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmForgotPasswordEmail(Guid code, [FromBody] ResetPasswordUserRequestDTO request)
        {
            await userService.ConfirmForgotPasswordEmail(code, request);
            return NoContent();
        }
    }
}
