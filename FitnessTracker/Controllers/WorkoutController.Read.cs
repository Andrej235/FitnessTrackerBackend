using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Services.Read;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class WorkoutController
    {
        [HttpGet("public/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            IEnumerable<Models.Workout> workouts = name is null
                ? await readRangeService.Get(x => x.IsPublic, offset, limit ?? 10, x => x.Include(x => x.Creator))
                : await readRangeService.Get(x => x.IsPublic && EF.Functions.Like(x.Name, $"%{name}%"), offset, limit ?? 10, x => x.Include(x => x.Creator));

            return Ok(workouts.Select(simpleResponseMapper.Map));
        }

        [HttpGet("public/simple/by/{username}")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBy(string username, [FromQuery] string? nameFilter, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            Guid userId = await userReadSingleSelectedService.Get(
                x => x.Id,
                x => x.Username == username);

            IEnumerable<Models.Workout> workouts = nameFilter is null
                ? await readRangeService.Get(x => x.CreatorId == userId && x.IsPublic, offset, limit ?? 10, x => x.Include(x => x.Creator))
                : await readRangeService.Get(x => x.CreatorId == userId && x.IsPublic && EF.Functions.Like(x.Name, $"%{nameFilter}%"), offset, limit ?? 10, x => x.Include(x => x.Creator));

            return Ok(workouts.Select(simpleResponseMapper.Map));
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("personal/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllPersonal([FromQuery] string? nameFilter, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            IEnumerable<Models.Workout> workouts = nameFilter is null
                ? await readRangeService.Get(x => x.CreatorId == userId, offset, limit ?? 10, x => x.Include(x => x.Creator).OrderByDescending(x => x.CreatedAt))
                : await readRangeService.Get(x => x.CreatorId == userId && EF.Functions.Like(x.Name, $"%{nameFilter}%"), offset, limit ?? 10, x => x.Include(x => x.Creator).OrderByDescending(x => x.CreatedAt));

            return Ok(workouts.Select(simpleResponseMapper.Map));
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("favorite/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllFavorites([FromQuery] string? nameFilter, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            IEnumerable<Models.FavoriteWorkout> workouts = nameFilter is null
                ? await favoriteReadRangeService.Get(x => x.UserId == userId && (x.Workout.IsPublic || x.Workout.CreatorId == userId), offset, limit ?? 10, x => x.Include(x => x.Workout).ThenInclude(x => x.Creator))
                : await favoriteReadRangeService.Get(x => x.UserId == userId && (x.Workout.IsPublic || x.Workout.CreatorId == userId) && EF.Functions.Like(x.Workout.Name, $"%{nameFilter}%"), offset, limit ?? 10, x => x.Include(x => x.Workout).ThenInclude(x => x.Creator));

            return Ok(workouts.Select(x => simpleResponseMapper.Map(x.Workout)));
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("liked/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllLiked([FromQuery] string? nameFilter, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            IEnumerable<Models.WorkoutLike> workouts = nameFilter is null
                ? await likeReadRangeService.Get(x => x.UserId == userId && (x.Workout.IsPublic || x.Workout.CreatorId == userId), offset, limit ?? 10, x => x.Include(x => x.Workout).ThenInclude(x => x.Creator))
                : await likeReadRangeService.Get(x => x.UserId == userId && (x.Workout.IsPublic || x.Workout.CreatorId == userId) && EF.Functions.Like(x.Workout.Name, $"%{nameFilter}%"), offset, limit ?? 10, x => x.Include(x => x.Workout).ThenInclude(x => x.Creator));

            return Ok(workouts.Select(x => simpleResponseMapper.Map(x.Workout)));
        }

        [HttpGet("{id:guid}/detailed")]
        [ProducesResponseType(typeof(DetailedWorkoutResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetailed(Guid id)
        {
            Guid? userId = User.Identity is ClaimsIdentity claimsIdentity
                ? claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is string userIdString
                    ? Guid.TryParse(userIdString, out Guid parsedUserId)
                        ? parsedUserId
                        : null
                    : null
                : null;

            var data = await readSingleSelectedService.Get(
                x => new
                {
                    likeCount = x.Likes.Count,
                    favoriteCount = x.Favorites.Count,
                    commentCount = x.Comments.Count,
                    isLiked = userId != null && x.Likes.Any(x => x.Id == userId),
                    isFavorite = userId != null && x.Favorites.Any(x => x.Id == userId),
                    workout = x,
                },
                x => x.Id == id,
                x => x.Include(x => x.Creator)
                      .Include(x => x.Sets)
                      .ThenInclude(x => x.Exercise));

            if (data is null)
                return NotFound();

            DetailedWorkoutResponseDTO mapped = detailedResponseMapper.Map(data.workout);

            if (!data.workout.IsPublic && data.workout.CreatorId != userId)
                return Forbid();

            IEnumerable<Models.CompletedWorkout> completed = await completedWorkoutReadRangeService.Get(
                criteria: x => x.UserId == userId && x.WorkoutId == id,
                limit: 1,
                queryBuilder: x => x.Include(x => x.CompletedSets).OrderByDescending(x => x.CompletedAt));

            if (!completed.Any())
                return Ok(mapped);

            Models.CompletedWorkout latest = completed.First();
            mapped.AlreadyAttempted = true;
            mapped.Sets = mapped.Sets.Select(set =>
            {
                Models.CompletedSet? completedSet = latest.CompletedSets.FirstOrDefault(x => x.SetId == set.Id);
                if (completedSet is null)
                    return set;

                set.WeightUsedLastTime = completedSet.WeightUsed;
                set.RepsCompletedLastTime = completedSet.RepsCompleted;
                return set;
            });

            return Ok(mapped);
        }
    }
}
