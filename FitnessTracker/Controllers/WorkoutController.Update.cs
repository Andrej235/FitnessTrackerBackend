using FitnessTracker.DTOs.Requests.Set;
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
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPatch("{id:guid}/baseinfo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBasicInformation(Guid id, [FromBody] UpdateWorkoutBaseInfoRequestDTO request)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out Guid userId))
                    return Unauthorized();

                Workout? workout = await readSingleService.Get(x => x.CreatorId == userId && x.Id == id);
                if (workout is null)
                    return NotFound();

                workout.Name = request.Name;
                workout.Description = request.Description;
                workout.IsPublic = request.IsPublic;

                await updateService.Update(workout);
                return NoContent();

            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest();
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPatch("{id:guid}/set/{setId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSet(Guid id, Guid setId, [FromBody] UpdateSetRequestDTO request)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out Guid userId))
                    return Unauthorized();

                Models.Workout? workout = await readSingleService.Get(x => x.CreatorId == userId && x.Id == id, x => x.Include(x => x.Sets));
                Models.Set? set = workout?.Sets.FirstOrDefault(x => x.Id == setId);

                if (set is null)
                    return NotFound();

                set.ExerciseId = request.ExerciseId;
                set.TopRepRange = request.TopRepRange;
                set.BottomRepRange = request.BottomRepRange;
                set.RiR = request.RiR;

                await setUpdateService.Update(set);
                return NoContent();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest();
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpDelete("{id:guid}/set/{setId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSet(Guid id, Guid setId)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out Guid userId))
                    return Unauthorized();

                Models.Workout? workout = await readSingleService.Get(x => x.CreatorId == userId && x.Id == id, x => x.Include(x => x.Sets));
                Models.Set? set = workout?.Sets.FirstOrDefault(x => x.Id == setId);

                if (workout is null || set is null)
                    return NotFound();

                _ = workout.Sets.Remove(set);
                await updateService.Update(workout);
                return NoContent();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest();
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost("{id:guid}/set")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateSet(Guid id, [FromBody] CreateSetRequestDTO request)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out Guid userId))
                    return Unauthorized();

                Models.Workout? workout = await readSingleService.Get(x => x.CreatorId == userId && x.Id == id, x => x.Include(x => x.Sets));
                if (workout is null)
                    return NotFound();

                Models.Set set = setCreateRequestMapper.Map(request);
                workout.Sets.Add(set);
                await updateService.Update(workout);
                return Created();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest();
            }
        }

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
