using FitnessTracker.DTOs.Responses.Exercises;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class ExerciseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SimpleExerciseResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] string? q, [FromQuery] int? limit, [FromQuery] int? offset, [FromQuery] string? include)
        {
            var exercises = await readQueryService.Get(q, offset, limit, include);
            return Ok(exercises.Select(simpleResponseMapper.Map));
        }

        [HttpGet("{id:int}/detailed")]
        [ProducesResponseType(typeof(DetailedExerciseResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var exercise = await readSingleService.Get(x => x.Id == id, "primarymusclegroups,secondarymusclegroups,primarymuscles,secondarymuscles,equipment");
            if (exercise is null)
                return NotFound();

            return Ok(detailedResponseMapper.Map(exercise));
        }
    }
}
