using FitnessTracker.DTOs.Requests.MuscleGroup;
using FitnessTracker.DTOs.Responses.MuscleGroup;
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
    [Route("api/musclegroup")]
    public class MuscleGroupController(ICreateService<MuscleGroup> createService,
                                       ICreateRangeService<MuscleGroup> createRangeService,
                                       IReadRangeService<MuscleGroup> readRangeService,
                                       IRequestMapper<CreateMuscleGroupRequestDTO, MuscleGroup> requestMapper,
                                       IResponseMapper<MuscleGroup, SimpleMuscleGroupResponseDTO> responseMapper) : ControllerBase
    {
        private readonly ICreateService<MuscleGroup> createService = createService;
        private readonly ICreateRangeService<MuscleGroup> createRangeService = createRangeService;
        private readonly IReadRangeService<MuscleGroup> readRangeService = readRangeService;
        private readonly IRequestMapper<CreateMuscleGroupRequestDTO, MuscleGroup> requestMapper = requestMapper;
        private readonly IResponseMapper<MuscleGroup, SimpleMuscleGroupResponseDTO> responseMapper = responseMapper;

        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMuscleGroupRequestDTO request)
        {
            await createService.Add(requestMapper.Map(request));
            return Ok();
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("range")]
        public async Task<IActionResult> Create([FromBody] IEnumerable<CreateMuscleGroupRequestDTO> request)
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
