using FitnessTracker.Emails.Confirmation;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers.Admin
{
    [Route("api/unsafe/user")]
    [ApiController]
    public class UNSAFE_UserController(IEmailConfirmationService emailConformationService) : ControllerBase
    {
        [Authorize(Roles = Role.Unverified)]
        [HttpGet("mail")]
        public async Task<IActionResult> GetMail([FromQuery] string mail)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            await emailConformationService.SendEmailConfirmation(mail, userId);
            return Ok();
        }

        [Authorize(Roles = Role.Unverified)]
        [HttpGet("confirm/{code:guid}")]
        public async Task<IActionResult> Confirm(Guid code)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var success = await emailConformationService.ConfirmEmail(userId, code);
            return success ? Ok("Email Confirmed") : BadRequest("Invalid code");
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("roletest")]
        public IActionResult RoleTest()
        {
            return Ok();
        }
    }
}
