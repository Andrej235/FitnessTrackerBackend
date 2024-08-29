using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.Models;
using FitnessTracker.Services.Read.Full;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class WorkoutController
    {
        [HttpPut("{id:guid}")]
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateWorkout(Guid id, [FromBody] UpdateFullWorkoutRequestDTO request)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out Guid userId))
                    return Unauthorized();

                Workout? workout = await readSingleService.Get(x => x.CreatorId == userId && x.Id == id, x => x.Include(x => x.Sets));
                if (workout is null)
                    return NotFound();

                workout.Name = request.Name;
                workout.Description = request.Description;
                workout.IsPublic = request.IsPublic;
                workout.Sets = request.Sets.Select(setCreateRequestMapper.Map).ToList();
                await updateService.Update(workout);

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
