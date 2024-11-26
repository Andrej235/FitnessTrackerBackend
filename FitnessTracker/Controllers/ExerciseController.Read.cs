using FitnessTracker.DTOs.Responses.Exercises;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class ExerciseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleExerciseResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] int? muscleGroupId, [FromQuery] int? equipmentId, [FromQuery] string? name, [FromQuery] bool? favoritesOnly, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Ok(await exerciseService.GetAll(null, muscleGroupId, equipmentId, favoritesOnly, name, offset, limit));
            else
                return Ok(await exerciseService.GetAll(userId, muscleGroupId, equipmentId, favoritesOnly, name, offset, limit));
        }

        [HttpGet("{id:int}/detailed")]
        [ProducesResponseType(typeof(DetailedExerciseResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            Guid userId = default;
            if (User.Identity is ClaimsIdentity claimsIdentity
                && claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is string userIdString)
                _ = Guid.TryParse(userIdString, out userId);

            DetailedExerciseResponseDTO exercise = await exerciseService.GetDetailed(id, userId);
            return Ok(exercise);
        }
    }
}
