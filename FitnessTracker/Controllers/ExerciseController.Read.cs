using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class ExerciseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? q, [FromQuery] int? limit, [FromQuery] int? offset, [FromQuery] string? include)
        {
            var exercises = await readQueryService.Get(q, offset, limit, include);
            return Ok(exercises.Select(detailedResponseMapper.Map));
        }
    }
}
