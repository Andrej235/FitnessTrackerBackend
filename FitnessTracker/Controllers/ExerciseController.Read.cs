using FitnessTracker.DTOs.Responses.Exercises;
using FitnessTracker.Models;
using FitnessTracker.Services.Read.Full;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class ExerciseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleExerciseResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] int? muscleGroupId, [FromQuery] int? equipmentId, [FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            List<Expression<Func<Exercise, bool>>> filters = [];

            if (name is not null)
                filters.Add(e => EF.Functions.Like(e.Name, $"%{name}%"));

            if (muscleGroupId is not null)
                filters.Add(e => e.PrimaryMuscleGroups.Any(m => m.Id == muscleGroupId) || e.SecondaryMuscleGroups.Any(m => m.Id == muscleGroupId));

            if (equipmentId is not null)
                filters.Add(e => e.Equipment.Any(eq => eq.Id == equipmentId));

            IEnumerable<Models.Exercise> exercises = await readRangeService.Get(
                filters.Combine() ?? (x => true),
                offset,
                limit ?? 10,
                x =>
                {
                    if (muscleGroupId is not null)
                        x = x.Include(x => x.PrimaryMuscleGroups).Include(x => x.SecondaryMuscleGroups);

                    if (equipmentId is not null)
                        x = x.Include(x => x.Equipment);

                    return x;
                });

            //TODO: Sort the results based on how many criteria they match
            return Ok(exercises.Select(simpleResponseMapper.Map));
        }

        [HttpGet("{id:int}/detailed")]
        [ProducesResponseType(typeof(DetailedExerciseResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            Models.Exercise? exercise = await readSingleService.Get(
                x => x.Id == id,
                x => x.Include(x => x.PrimaryMuscleGroups)
                      .Include(x => x.SecondaryMuscleGroups)
                      .Include(x => x.PrimaryMuscles)
                      .Include(x => x.SecondaryMuscles)
                      .Include(x => x.Equipment)
                );

            if (exercise is null)
                return NotFound();

            DetailedExerciseResponseDTO mapped = detailedResponseMapper.Map(exercise);
            mapped.Favorites = await favoriteCountService.Count(x => x.ExerciseId == id);

            if (User.Identity is ClaimsIdentity claimsIdentity
                && claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is string userIdString
                && Guid.TryParse(userIdString, out Guid userId))
                mapped.IsFavorite = favoriteReadSingleService.Get(x => x.UserId == userId && x.ExerciseId == id) is not null;

            return Ok(mapped);
        }
    }
}
