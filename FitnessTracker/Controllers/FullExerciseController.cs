using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Models;
using FitnessTracker.Services.Mapping;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Controllers
{
    [Route("api/fullexercise")]
    [ApiController]
    public class FullExerciseController(IReadService<Exercise> readService, IEntityMapper<Exercise, object> mapper) : ControllerBase
    {
        private readonly IEntityMapper<Exercise, object> mapper = mapper;
        public IReadService<Exercise> ReadService { get; } = readService;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q)
        {
            var exercises = await ReadService.Get(q, offset, limit, include);
            return Ok(exercises.Select(mapper.Map).ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, [FromQuery] string? include)
        {
            var exercise = await ReadService.Get(id, include);
            if (exercise is null)
                return NotFound($"Entity with id {id} was not found.");

            return Ok(mapper.Map(exercise));
        }
    }
}
