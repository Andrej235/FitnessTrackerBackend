using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.Services.Read;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class SplitController
    {
        [HttpGet("public/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSimplePublic([FromQuery] string? name)
        {
            IEnumerable<Models.Split> splits = name is null
                ? await readRangeService.Get(x => x.IsPublic, 0, 10, x => x.Include(x => x.Creator))
                : await readRangeService.Get(x => x.IsPublic && EF.Functions.Like(x.Name, $"%{name}%"), 0, 10, x => x.Include(x => x.Creator));

            return Ok(splits.Select(simpleResponseMapper.Map));
        }

        [HttpGet("public/simple/by/{userId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSimplePublic(Guid userId, [FromQuery] string? name)
        {
            IEnumerable<Models.Split> splits = name is null
                ? await readRangeService.Get(x => x.CreatorId == userId && x.IsPublic, 0, 10, x => x.Include(x => x.Creator))
                : await readRangeService.Get(x => x.CreatorId == userId && x.IsPublic && EF.Functions.Like(x.Name, $"%{name}%"), 0, 10, x => x.Include(x => x.Creator));

            return Ok(splits.Select(simpleResponseMapper.Map));
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("personal/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllSimplePersonal([FromQuery] string? name)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            IEnumerable<Models.Split> splits = name is null
                ? await readRangeService.Get(x => x.CreatorId == userId, 0, 10, x => x.Include(x => x.Creator))
                : await readRangeService.Get(x => x.CreatorId == userId && EF.Functions.Like(x.Name, $"%{name}%"), 0, 10, x => x.Include(x => x.Creator));

            return Ok(splits.Select(simpleResponseMapper.Map));
        }

        [Authorize]
        [HttpGet("{id:guid}/detailed")]
        [ProducesResponseType(typeof(DetailedSplitResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetailed(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            Models.Split? split = await readSingleService.Get(
                x => x.Id == id,
                x => x.Include(x => x.Creator)
                      .Include(x => x.Workouts)
                      .ThenInclude(x => x.Workout)
                      .ThenInclude(x => x.Creator)
                      .Include(x => x.Likes)
                      .Include(x => x.Favorites)
                      .Include(x => x.Comments));

            if (split is null)
                return NotFound();

            if (!split.IsPublic && split.CreatorId != userId)
                return Unauthorized();

            DetailedSplitResponseDTO mapped = detailedResponseMapper.Map(split);
            mapped.IsLiked = split.Likes.Any(x => x.Id == userId);
            mapped.IsFavorited = split.Favorites.Any(x => x.Id == userId);
            return Ok(mapped);
        }


        [HttpGet("usedby/{username}/detailed")]
        [ProducesResponseType(typeof(DetailedUserSplitResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetailedUsedBy(string username)
        {
            var user = await userReadSingleService.Get(
                x => new
                {
                    x.Id,
                    x.SplitId,
                    x.Settings.PublicStreak
                },
                x => x.Username == username,
                x => x.Include(x => x.Settings));

            if (user is null)
                return NotFound();

            if (!user.PublicStreak)
                return Forbid();

            Models.Split? split = await readSingleService.Get(
                x => x.Id == user.SplitId,
                x => x.Include(x => x.Creator)
                      .Include(x => x.Workouts)
                      .ThenInclude(x => x.Workout));

            if (split is null)
                return NotFound();

            if (!split.IsPublic && split.CreatorId != user.Id)
                return Unauthorized();

            DetailedUserSplitResponseDTO mapped = detailedUserSplitResponseMapper.Map(split);
            return Ok(mapped);
        }
    }
}
