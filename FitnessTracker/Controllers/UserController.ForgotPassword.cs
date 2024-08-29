using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [HttpPost("me/forgotpassword")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SendForgotPasswordEmail([FromBody] SendForgotPasswordEmailRequestDTO request)
        {
            try
            {
                Models.User? user = await readSingleService.Get(x => x.Email == request.Email);
                if (user is null)
                    return NotFound();

                await passwordResetEmailSender.SendEmail(user.Email, user.Id);
                return Created();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
            }
        }

        [HttpPatch("me/forgotpassword/{code:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmForgotPasswordEmail(Guid code, [FromBody] ResetPasswordUserRequestDTO request)
        {
            try
            {
                if (request.NewPassword.Length < 8)
                    return BadRequest("Password must be at least 8 characters long");

                Models.User? user = await readSingleService.Get(x => x.Email == request.Email);
                if (user is null)
                    return NotFound();

                bool success = await passwordResetEmailService.ResetPassword(user.Id, code, request.NewPassword);
                return success ? NoContent() : BadRequest("Invalid code");
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
            }
        }
    }
}
