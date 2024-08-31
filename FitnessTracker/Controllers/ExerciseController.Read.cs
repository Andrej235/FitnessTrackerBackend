using FitnessTracker.DTOs.Responses.Exercises;
using FitnessTracker.Services.Read.Full;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class ExerciseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleExerciseResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] int? muscleGroupId, [FromQuery] int? equipmentId, [FromQuery] string? name, [FromQuery] bool? favoriteOnly, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            ICollection<string> include = [];
            string query = "strict=true;";

            if (name is not null)
            {
                query += $"name={name};";
            }

            if (muscleGroupId is not null)
            {
                include.Add("primarymusclegroups,secondarymusclegroups");
                query += $"usesmusclegroup={muscleGroupId};";
            }

            if (equipmentId is not null)
            {
                include.Add("equipment");
                query += $"usesequipment={equipmentId};";
            }

            if (favoriteOnly is not null && favoriteOnly.Value)
            {
                if (User.Identity is ClaimsIdentity claimsIdentity
                    && claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is string userIdString
                    && Guid.TryParse(userIdString, out Guid userId))
                {
                    include.Add("favorites");
                    query += $"favoritedby={userId};";
                }
            }

            IEnumerable<Models.Exercise> exercises = await readQueryService.Get(query, offset, limit, string.Join(',', include));
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
