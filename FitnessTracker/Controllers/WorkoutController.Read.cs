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
        public async Task<IActionResult> GetAllSimplePublic([FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            IEnumerable<Models.Workout> workouts = name is null
                ? await readRangeService.Get(x => x.IsPublic, offset, limit ?? 10, x => x.Include(x => x.Creator))
                : await readRangeService.Get(x => x.IsPublic && EF.Functions.Like(x.Name, $"%{name}%"), offset, limit ?? 10, x => x.Include(x => x.Creator));

            return Ok(workouts.Select(simpleResponseMapper.Map));
        }

        [HttpGet("public/simple/by/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSimplePublic(Guid userId, [FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            IEnumerable<Models.Workout> workouts = name is null
                ? await readRangeService.Get(x => x.CreatorId == userId && x.IsPublic, offset, limit ?? 10, x => x.Include(x => x.Creator))
                : await readRangeService.Get(x => x.CreatorId == userId && x.IsPublic && EF.Functions.Like(x.Name, $"%{name}%"), offset, limit ?? 10, x => x.Include(x => x.Creator));

            return Ok(workouts.Select(simpleResponseMapper.Map));
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("personal/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllSimplePersonal([FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            IEnumerable<Models.Workout> workouts = name is null
                ? await readRangeService.Get(x => x.CreatorId == userId, offset, limit ?? 10, x => x.Include(x => x.Creator).OrderByDescending(x => x.CreatedAt))
                : await readRangeService.Get(x => x.CreatorId == userId && EF.Functions.Like(x.Name, $"%{name}%"), offset, limit ?? 10, x => x.Include(x => x.Creator).OrderByDescending(x => x.CreatedAt));

            return Ok(workouts.Select(simpleResponseMapper.Map));
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("favorite/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllSimpleFavorites([FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            IEnumerable<Models.FavoriteWorkout> workouts = name is null
                ? await favoriteReadRangeService.Get(x => x.UserId == userId && (x.Workout.IsPublic || x.Workout.CreatorId == userId), offset, limit ?? 10, x => x.Include(x => x.Workout).ThenInclude(x => x.Creator))
                : await favoriteReadRangeService.Get(x => x.UserId == userId && (x.Workout.IsPublic || x.Workout.CreatorId == userId) && EF.Functions.Like(x.Workout.Name, $"%{name}%"), offset, limit ?? 10, x => x.Include(x => x.Workout).ThenInclude(x => x.Creator));

            return Ok(workouts.Select(x => simpleResponseMapper.Map(x.Workout)));
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("liked/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllSimpleLikes([FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            IEnumerable<Models.WorkoutLike> workouts = name is null
                ? await likeReadRangeService.Get(x => x.UserId == userId && (x.Workout.IsPublic || x.Workout.CreatorId == userId), offset, limit ?? 10, x => x.Include(x => x.Workout).ThenInclude(x => x.Creator))
                : await likeReadRangeService.Get(x => x.UserId == userId && (x.Workout.IsPublic || x.Workout.CreatorId == userId) && EF.Functions.Like(x.Workout.Name, $"%{name}%"), offset, limit ?? 10, x => x.Include(x => x.Workout).ThenInclude(x => x.Creator));

            return Ok(workouts.Select(x => simpleResponseMapper.Map(x.Workout)));
        }

        [HttpGet("{id:guid}/detailed")]
        [ProducesResponseType(typeof(DetailedWorkoutResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetailed(Guid id)
        {
            Models.Workout? workout = await readSingleService.Get(x => x.Id == id, x => x.Include(x => x.Creator)
                                                                                         .Include(x => x.Sets)
                                                                                         .ThenInclude(x => x.Exercise));

            if (workout is null)
                return NotFound();

            DetailedWorkoutResponseDTO mapped = detailedResponseMapper.Map(workout);

            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return workout.IsPublic ? Ok(mapped) : Unauthorized();

            if (!workout.IsPublic && workout.CreatorId != userId)
                return Unauthorized();

            mapped.IsLiked = (await likeReadSingleService.Get(x => x.UserId == userId && x.WorkoutId == id)) is not null;
            mapped.IsFavorited = (await favoriteReadSingleService.Get(x => x.UserId == userId && x.WorkoutId == id)) is not null;
            mapped.LikeCount = await likeCountService.Count(x => x.WorkoutId == id);
            mapped.FavoriteCount = await favoriteCountService.Count(x => x.WorkoutId == id);
            mapped.CommentCount = await commentCountService.Count(x => x.WorkoutId == id);

            IEnumerable<Models.CompletedWorkout> completed = await completedWorkoutReadSingleService.Get(
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
