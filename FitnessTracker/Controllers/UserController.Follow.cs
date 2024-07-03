using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpPost("{id}/follow")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Follow(Guid id)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out var userId))
                    return Unauthorized();

                if (userId == id)
                    return BadRequest("Cannot follow yourself");

                await followCreateService.Add(new()
                {
                    FollowerId = userId,
                    FolloweeId = id
                });

                return NoContent();

            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest();
            }
        }

        [Authorize]
        [HttpDelete("{id}/follow")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Unfollow(Guid id)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out var userId))
                    return Unauthorized();

                if (userId == id)
                    return BadRequest("Cannot unfollow yourself");

                await followDeleteService.Delete(x => x.FollowerId == userId && x.FolloweeId == id);
                return NoContent();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest();
            }
        }
    }
}
