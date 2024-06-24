using FitnessTracker.DTOs.Requests.Muscle;
using FitnessTracker.DTOs.Responses.Muscle;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.Read.ExpressionBased;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Route("api/muscle")]
    public class MuscleController(ICreateService<Muscle> createService,
                                  ICreateRangeService<Muscle> createRangeService,
                                  IReadRangeService<Muscle> readRangeService,
                                  IRequestMapper<CreateMuscleRequestDTO, Muscle> requestMapper,
                                  IResponseMapper<Muscle, SimpleMuscleResponseDTO> responseMapper) : ControllerBase
    {
        private readonly ICreateService<Muscle> createService = createService;
        private readonly ICreateRangeService<Muscle> createRangeService = createRangeService;
        private readonly IReadRangeService<Muscle> readRangeService = readRangeService;
        private readonly IRequestMapper<CreateMuscleRequestDTO, Muscle> requestMapper = requestMapper;
        private readonly IResponseMapper<Muscle, SimpleMuscleResponseDTO> responseMapper = responseMapper;

        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMuscleRequestDTO request)
        {
            await createService.Add(requestMapper.Map(request));
            return Ok();
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("range")]
        public async Task<IActionResult> Create([FromBody] IEnumerable<CreateMuscleRequestDTO> request)
        {
            await createRangeService.Add(request.Select(requestMapper.Map));
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? limit, [FromQuery] int? offset, [FromQuery] string? include)
        {
            var muscleGroups = await readRangeService.Get(x => true, offset, limit, include);
            return Ok(muscleGroups.Select(responseMapper.Map));
        }
    }
}
