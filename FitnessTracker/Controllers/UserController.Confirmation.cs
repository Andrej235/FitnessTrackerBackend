using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize(Roles = Role.Unverified)]
        [HttpPost("resendconfirmationemail")]
        public async Task<IActionResult> ResendConfirmationEmail()
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out var userId))
                    return Unauthorized();

                var user = await readSingleService.Get(x => x.Id == userId, "none");
                if (user is null)
                    return Unauthorized();

                await emailConfirmationSender.SendEmailConfirmation(user.Email, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
            }
        }

        [Authorize(Roles = Role.Unverified)]
        [HttpPost("confirm/{code:guid}")]
        public async Task<IActionResult> ConfirmEmail(Guid code)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out var userId))
                    return Unauthorized();

                var success = await emailConfirmationService.ConfirmEmail(userId, code);
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
