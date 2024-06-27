using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize(Roles = Role.Unverified)]
        [HttpPost("me/resendconfirmationemail")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
                return Created();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
            }
        }

        [Authorize(Roles = Role.Unverified)]
        [HttpPatch("me/confirmemail/{code:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ConfirmEmail(Guid code)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out var userId))
                    return Unauthorized();

                var success = await emailConfirmationService.ConfirmEmail(userId, code);
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
