using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("forgotpassword")]
        public async Task<IActionResult> SendForgotPasswordEmail()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var user = await readSingleService.Get(x => x.Id == userId, "none");
            if (user is null)
                return Unauthorized();

            await passwordResetEmailSender.SendEmail(user.Email, userId);
            return Ok();
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("forgotpassword/{code:guid}")]
        public async Task<IActionResult> ConfirmForgotPasswordEmail(Guid code, [FromBody] ResetPasswordUserRequestDTO request)
        {
            try
            {
                if (request.NewPassword.Length < 8)
                    return BadRequest("Password must be at least 8 characters long");

                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out var userId))
                    return Unauthorized();

                var success = await passwordResetEmailService.ResetPassword(userId, code, request.NewPassword);
                return success ? Ok("Email Confirmed") : BadRequest("Invalid code");
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
            }
        }
    }
}
